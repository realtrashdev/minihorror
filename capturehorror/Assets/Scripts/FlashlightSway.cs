using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] public float rotateSmooth;
    [SerializeField] public float moveSmooth;
    [SerializeField] private float swayMultiplier;

    [HideInInspector] public bool doSway = true;

    public PlayerMovement playerMovement;
    public Rigidbody rb;
    public Transform playerCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (doSway)
        {
            Sway();
        }
    }

    void Sway()
    {
        Vector3 movePos = new Vector3((Input.GetAxis("Horizontal") * playerMovement.moveSpeed) / 25, 0, (-Input.GetAxis("Vertical") * playerMovement.moveSpeed) / 25);

        //get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        //calculate target rotation
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        //move & rotate
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotateSmooth * Time.deltaTime);
        transform.localPosition = Vector3.Lerp(transform.localPosition, movePos, moveSmooth * Time.deltaTime);
    }
}
