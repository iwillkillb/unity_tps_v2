using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.EventSystems;

public class PlayerCamera : MonoBehaviour
{
    #region Singletone
    public static PlayerCamera instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of PlayerCamera found.");
            return;
        }

        instance = this;
    }
    #endregion

    // How to Use?
    // 1. Make this hierarchy
    //  Component's object
    //      > Y Axis (0, 0, 0) All Zero Position
    //          > X Axis (0, 0, 0) All Zero Position
    //              > Main Camera
    //              > Zoom out limit (x, y, z) Free Position
    // 2. Set trackingTarget and other public objects.

    [Header("Transform Connection")]
    public Transform cam;               // Camera's moving position by zooming
    public Transform joint;             // Camera's rotation
    public Transform FarestPoint;       // Camera's farest position
    public Transform NearestPoint;      // Camera's nearest position

    [Header("Terrain Check")]
    public LayerMask terrainLayerMask;
    RaycastHit hit;

    // Input
    float inputAxisX;
    float inputAxisY;
    float inputAxisZ;

    [Header("Position")]
    [Tooltip("If trackingTarget is Null, it is not tracked.")]
    public Transform trackingTarget;

    // Camera's rotation sequence
    // Angle -> Quaternion -> Transform
    [Header("Rotation")]
    public bool useRotationXLimit = true;
    [Range(-90f, 0f)] public float minXAxis = -45f;           // X Axis has limit.
    [Range(0f, 90f)] public float maxXAxis = 75f;
    float angleXAxis;
    public bool useRotationYLimit = false;
    [Range(-180f, 0f)] public float minYAxis = -90f;           // X Axis has limit.
    [Range(0f, 180f)] public float maxYAxis = 90f;
    float angleYAxis;
    public float rotationSensitivity = 2f;

    [Header("Zoom")]
    [Range(0f, 1f)] public float zoomAxisMin = 0f;
    [Range(0f, 1f)] public float zoomAxisMax = 1f;
    float zoomAxis = 1f;                    // Default zoom axis, Nearest : 0






    private void Update()
    {
        /*
        if (EventSystem.current.IsPointerOverGameObject())
        {
            inputAxisX = 0;
            inputAxisY = 0;
            inputAxisZ = 0;
            return;
        }*/

        // Take input
        inputAxisX = Input.GetAxis("Mouse X");
        inputAxisY = Input.GetAxis("Mouse Y");
        inputAxisZ = Input.GetAxis("Mouse ScrollWheel");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Positioning
        if (trackingTarget != null)
            transform.position = trackingTarget.position;

        // Shift Key -> Camera Stop
        if (Input.GetKey(KeyCode.LeftShift))
        {
            return;
        }

        // XY Axis Rotation (with clamping)
        angleXAxis -= inputAxisY * rotationSensitivity;
        if (useRotationXLimit)
            angleXAxis = Mathf.Clamp(angleXAxis, minXAxis, maxXAxis);

        angleYAxis += inputAxisX * rotationSensitivity;
        if (useRotationYLimit)
            angleYAxis = Mathf.Clamp(angleYAxis, minYAxis, maxYAxis);

        joint.localRotation = Quaternion.Euler(angleXAxis, angleYAxis, 0f);

        // Camera Collision Check and Zoom ---------------------------------------------------------------

        // 1. Input
        // 2. Check collider in camera zoom line
        // 3. Compare Zoom axis by collision to Default zoom axis.
        // 4. Select lesser one.

        // Take Input.
        zoomAxis = Mathf.Clamp(zoomAxis - inputAxisZ, zoomAxisMin, zoomAxisMax);

        // Is there something(Without itself) between camera and character?
        // Don't put trackingTarget's layer in terrainLayerMask!!
        float zoomAxisByDistanceToCollider = 1f;

        Vector3 camPosByZoomAxis = Vector3.Lerp(NearestPoint.position, FarestPoint.position, zoomAxis);
        float lengthOfZoomLine = Vector3.Distance(joint.position, camPosByZoomAxis);    // Length of camera's zooming line

        // Is there Terrain? Check collider.
        if (Physics.Linecast(joint.position, camPosByZoomAxis, out hit, terrainLayerMask))
        {
            // Lesser Zoom Axis -> Near
            zoomAxisByDistanceToCollider = hit.distance / lengthOfZoomLine;
        }

        cam.position = Vector3.Lerp(joint.position, camPosByZoomAxis, zoomAxisByDistanceToCollider);
        // -----------------------------------------------------------------------------------------------
    }
}
