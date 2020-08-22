﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBMovement1 : MonoBehaviour
{
    // Components
    public Animator _Animator;
    Rigidbody _Rigidbody;
    CapsuleCollider _CapsuleCollider;
    PlayerInput playerInput;

    // Input field
    public float inputAxisHor { get; set; }
    public float inputAxisVer { get; set; }
    public bool inputJump { get; set; }

    [Header("Move")]
    public float moveSpeed = 7f;
    public float acceleration = 20f;

    [Header("Rotation")]
    public float rotationSlerp = 5f;        // This is used by smooth rotation If you don't use NavMeshAgent's rotation.
    public bool isStaringFront = true;

    [Header("Jump")]
    public float jumpForce = 10f;

    [Header("Terrain Check")]
    public LayerMask terrainLayer;
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.1f;
    public bool isGrounded;




    void Awake()
    {
        // Components connecting
        _Rigidbody = GetComponent<Rigidbody>();
        _CapsuleCollider = GetComponent<CapsuleCollider>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        inputAxisHor = playerInput.axisHor;
        inputAxisVer = playerInput.axisVer;
        inputJump = playerInput.axisJump;

        // Ground Check
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, terrainLayer);

        // Jump
        if (isGrounded && inputJump)
        {
            _Rigidbody.AddRelativeForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }

        Movement();
        Rotation();

        Dash();
    }

    Vector3 GetGroundNormal()
    {
        // Slope Check
        RaycastHit slopeHit;
        Vector3 groundNormal = Vector3.up;
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 10f))
        {
            groundNormal = slopeHit.normal;
        }

        return groundNormal;
    }

    void Movement()
    {
        Vector3 moveAxis = Vector3.forward * inputAxisVer + Vector3.right * inputAxisHor;
        Quaternion moveDir = Quaternion.Euler(Vector3.up * Camera.main.transform.eulerAngles.y);
        Quaternion groundNormal = Quaternion.FromToRotation(Vector3.up, GetGroundNormal());

        // Wall check
        // Aerial collision -> Wall sliding -> There is error when collision with thin terrian.
        if (inputAxisHor != 0f || inputAxisVer != 0f)
        {
            Vector3 capsuleCastPoint1 = transform.position + _CapsuleCollider.center - (Vector3.up * (_CapsuleCollider.height * 0.5f - _CapsuleCollider.radius * 2f));
            Vector3 capsuleCastPoint2 = transform.position + _CapsuleCollider.center + (Vector3.up * (_CapsuleCollider.height * 0.5f - _CapsuleCollider.radius * 2f));
            RaycastHit hitBody;

            // Aerial collision -> Zero speed
            if (Physics.CapsuleCast(capsuleCastPoint1, capsuleCastPoint2, _CapsuleCollider.radius, moveDir * moveAxis.normalized, out hitBody, _CapsuleCollider.radius))
            {
                // WWWWW                 WWWWW
                // WWW                     WWW
                // W                         W
                //   C                     C
                //   |                     |
                //  -30 -> rotate 60       30 -> rotate -60
                // Angle perpendicular to the wall
                float angleToWall = Vector3.SignedAngle(moveDir * moveAxis, -hitBody.normal, Vector3.up);

                // New direction setting : Collision with wall -> Move side of wall(Perpendicular to the wall)
                if (angleToWall > 0f)
                    moveDir = Quaternion.Euler(Vector3.up * (Camera.main.transform.eulerAngles.y + (-90 + angleToWall)));
                else
                    moveDir = Quaternion.Euler(Vector3.up * (Camera.main.transform.eulerAngles.y + (90 + angleToWall)));

                // Speed reduce by friction
                moveAxis *= Mathf.Abs(angleToWall) / 90f;
            }
        }

        // Movement
        transform.position += groundNormal * moveDir * moveAxis * moveSpeed * Time.deltaTime;
    }

    void Rotation()
    {
        // Character Rotation : Character and camera move in the apposite direction (in Y axis).
        // Example : 
        // 1. Input Right key -> inputAngle is 90
        // 2. Character moves Right -> Character rotates (inputAngle + Camera's current angle)

        // inputAngle : Front 0, Back 180, Left -90, Right 90
        float inputAngle = Mathf.Atan2(inputAxisHor, inputAxisVer) * Mathf.Rad2Deg;

        // Rotation backup
        Quaternion newRot = transform.rotation;  // Set new direction rotation value.

        // Stare
        // True  : Character stares Camera's direction.
        // False : The character looks in the direction in which it moves.
        if (isStaringFront)
        {
            newRot = Quaternion.Euler(Vector3.up * Camera.main.transform.eulerAngles.y);
        }
        // No stare -> Rotate only moving
        else if (inputAxisHor != 0f || inputAxisVer != 0f)
        {
            newRot = Quaternion.Euler(Vector3.up * (inputAngle + Camera.main.transform.eulerAngles.y));
        }

        // Actual Rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, newRot, rotationSlerp * Time.deltaTime);
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _Rigidbody.AddRelativeForce(Vector3.forward * jumpForce, ForceMode.VelocityChange);
        }
    }
}