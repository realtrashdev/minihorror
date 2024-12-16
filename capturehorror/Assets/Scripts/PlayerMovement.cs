using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public bool canMove;

    //KeyCode sprint;

    [Header("References")]
    [SerializeField] CinemachineCamera playerCamera;
    CapsuleCollider collision;

    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float moveSpeed;
    [SerializeField] float drag;
    bool sprinting;

    [Header("Crouching")]
    [SerializeField] float heightSpeed;
    [SerializeField] float crouchY;
    bool crouching;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask ground;
    public bool grounded;

    [Header("Camera Effects")]
    [SerializeField] float defaultFOV;
    [SerializeField] float sprintFOV;
    [SerializeField] float fovSmoothing;

    Rigidbody rb;

    float inputHorizontal;
    float inputVertical;
    Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = walkSpeed;

        collision = GetComponentInChildren<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        CheckGrounded();
        CheckCrouch();
        CheckSprint();
        SetMovementState();

        if (rb.linearVelocity != Vector3.zero)
        {
            SpeedControl();
        }

        Debug.Log("Speed: " + moveSpeed);
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            MovePlayer();
            MovementEffects();
        }
    }

    void GetInput()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
    }

    void MovePlayer()
    {
        moveDirection = transform.forward * inputVertical + transform.right * inputHorizontal;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    void CheckGrounded()
    {
        //check
        grounded = Physics.Raycast(transform.position, Vector3.down, (playerHeight * 0.5f) + 0.2f, ground);

        //apply movement drag
        switch (grounded)
        {
            case true:
                rb.linearDamping = drag;
                break;
            case false:
                rb.linearDamping = 0f;
                break;
        }
    }

    void CheckCrouch()
    {
        if (crouching && Physics.Raycast(transform.position, Vector3.up, (playerHeight * 0.5f) + 0.1f, ground))
        {
            return;
        }

        switch (Input.GetKey(KeyCode.LeftControl))
        {
            case true:
                crouching = true;
                break;
            case false:
                crouching = false;
                break;
        }
    }

    void CheckSprint()
    {
        if (rb.linearVelocity == Vector3.zero || crouching)
        {
            sprinting = false;
            return;
        }

        switch (Input.GetKey(KeyCode.LeftShift))
        {
            case true:
                sprinting = true;
                break;
            case false:
                sprinting = false;
                break;
        }
    }

    void SetMovementState()
    {
        if (crouching)
        {
            playerHeight = 1.5f;
            moveSpeed = crouchSpeed;
        }

        else if (sprinting)
        {
            playerHeight = 1.9f;
            moveSpeed = sprintSpeed;
        }

        else
        {
            playerHeight = 2;
            moveSpeed = walkSpeed;
        }
    }

    void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        //limit speed if necessary
        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
        }
    }

    void MovementEffects()
    {
        Vector3 camPos = playerCamera.transform.localPosition;

        playerCamera.transform.localPosition = Vector3.Lerp(camPos, new Vector3(camPos.x, playerHeight - 1.5f, camPos.z), heightSpeed * Time.deltaTime);
        collision.height = Mathf.Lerp(collision.height, playerHeight, heightSpeed * Time.deltaTime);

        switch (sprinting)
        {
            case true:
                playerCamera.Lens.FieldOfView = Mathf.Lerp(playerCamera.Lens.FieldOfView, sprintFOV, fovSmoothing * Time.deltaTime);
                break;
            case false:
                playerCamera.Lens.FieldOfView = Mathf.Lerp(playerCamera.Lens.FieldOfView, defaultFOV, fovSmoothing * Time.deltaTime);
                break;
        }
    }



    /*void GetKeybinds()
    {
        string keyName;
        keyName = PlayerPrefs.GetString("keybindSprint", "Shift");
        sprint = (KeyCode)Enum.Parse(typeof(KeyCode), keyName);
    }*/
}
