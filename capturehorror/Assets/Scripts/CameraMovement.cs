using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform playerTransform;

    [Header("Settings")]
    [SerializeField] bool smoothRotation;

    [Header("Movement")]
    [SerializeField] float xSens;
    [SerializeField] float ySens;
    [SerializeField] float lookSmoothing;

    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        RotateCamera();
    }

    void GetInput()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * xSens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * ySens;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }

    void RotateCamera()
    {
        playerTransform.rotation = Quaternion.Euler(0, yRotation, 0);
        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);

        switch (smoothRotation)
        {
            case true:
                //lerp camera movement
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, lookSmoothing * Time.deltaTime);
                break;
            case false:
                //precise camera movement
                transform.rotation = rotation;
                break;
        }
    }
}
