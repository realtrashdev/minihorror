using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public bool canMove;

    //KeyCode sprint;

    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    float moveSpeed;
    [SerializeField] float drag;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask ground;
    bool grounded;

    Rigidbody rb;

    float inputHorizontal;
    float inputVertical;
    Vector3 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = walkSpeed;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        CheckGrounded();
        CheckSprint();
        SpeedControl();

        Debug.Log("Speed: " + moveSpeed);
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            MovePlayer();
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

    void CheckSprint()
    {
        switch (Input.GetKey(KeyCode.LeftShift))
        {
            case true:
                moveSpeed = sprintSpeed;
                break;
            case false:
                moveSpeed = walkSpeed;
                break;
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



    /*void GetKeybinds()
    {
        string keyName;
        keyName = PlayerPrefs.GetString("keybindSprint", "Shift");
        sprint = (KeyCode)Enum.Parse(typeof(KeyCode), keyName);
    }*/
}
