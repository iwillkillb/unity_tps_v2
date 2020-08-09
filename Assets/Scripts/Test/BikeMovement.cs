using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BikeMovement : MonoBehaviour
{
    Rigidbody _Rigidbody;


    [Header("Ground Check")]
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;
    bool isGrounded = false;

    [Header("Slope Check")]
    public Transform slopeCheckPoint;
    public float slopeCheckRange = 5f;
    public float slopeForce = 1f;

    [Header("Lean")]
    public bool leanBodyWhenRotate = true;
    public float rotationLeanAngle = 30f;
    public float leanSpeed = 1f;
    Quaternion currentLean;
    Quaternion goalLean;

    [Header("Movement")]
    public float maxSpeed = 50f;
    public float acceleration = 50f;
    public float curveDegree = 90f;
    public float jumpForce = 50f;
    Vector3 moveVector;





    // Input field
    public float inputAxisHor { get; set; }
    public float inputAxisVer { get; set; }
    public bool inputJump { get; set; }






    // Start is called before the first frame update
    void Start()
    {
        _Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Input
        inputAxisHor = Input.GetAxis("Horizontal");
        inputAxisVer = Input.GetAxis("Vertical");
        inputJump = Input.GetButtonDown("Jump");

        // Ground Check
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayer);

        // Slope Check
        RaycastHit slopeHit;
        Vector3 groundNormal = Vector3.up;
        if (Physics.Raycast(slopeCheckPoint.position, -slopeCheckPoint.up, out slopeHit, slopeCheckRange, groundLayer))
        {
            groundNormal = slopeHit.normal;
        }

        // Slope Force -> Moving on slope without bouncing.
        if (!inputJump && !isGrounded)
        {
            _Rigidbody.AddForce(-groundNormal * slopeForce, ForceMode.VelocityChange);
        }

        // Lean
        currentLean = transform.rotation;
        goalLean = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;
        if (leanBodyWhenRotate)
        {
            goalLean *= Quaternion.Euler(Vector3.forward * (-inputAxisHor * rotationLeanAngle));
        }
        transform.rotation = Quaternion.Lerp(currentLean, goalLean, leanSpeed * Time.deltaTime);

        // Curve
        if (inputAxisHor != 0f)
        {
            transform.Rotate(Vector3.up * inputAxisHor * curveDegree * Time.deltaTime);
        }

        // Accel
        if ((inputAxisVer != 0f) && _Rigidbody.velocity.magnitude < maxSpeed && isGrounded)
        {
            _Rigidbody.AddRelativeForce((Vector3.forward * inputAxisVer) * acceleration, ForceMode.Acceleration);
        }

        // Jump on ground
        if (inputJump && isGrounded)
        {
            _Rigidbody.AddRelativeForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }
}
