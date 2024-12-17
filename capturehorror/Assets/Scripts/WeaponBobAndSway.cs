using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBobAndSway : MonoBehaviour
{
    [Header("External References")]
    [SerializeField] PlayerMovement mover;
    [SerializeField] Rigidbody rb;
    ItemManager itemManager;

    [Header("Settings")]
    public bool sway = true;
    public bool swayRotation = true;
    public bool bobOffset = true;
    public bool bobSway = true;
    [SerializeField] float swaySpeed;

    [Header("Multiplier Presets")]
    [SerializeField] Vector3 standMultiplier;
    [SerializeField] Vector3 walkMultiplier;
    [SerializeField] Vector3 sprintMultiplier;
    [SerializeField] Vector3 crouchMultiplier;


    // Start is called before the first frame update
    void Start()
    {
        itemManager = GetComponent<ItemManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (itemManager.switching)
        {
            return;
        }

        GetMultiplier();
        GetInput();

        Sway();
        SwayRotation();
        BobOffset();
        BobRotation();

        CompositePositionRotation();
    }

    void GetMultiplier()
    {
        if (rb.linearVelocity == Vector3.zero)
        {
            multiplier = standMultiplier;
        }

        else if (mover.sprinting)
        {
            multiplier = sprintMultiplier;
        }

        else if (mover.crouching || mover.slowed)
        {
            multiplier = crouchMultiplier;
        }

        else
        {
            multiplier = walkMultiplier;
        }
    }

    //store inputs
    Vector2 walkInput; //keyboard
    Vector2 lookInput; //mouse

    void GetInput()
    {
        walkInput.x = rb.linearVelocity.x;
        walkInput.y = rb.linearVelocity.y;
        walkInput = walkInput.normalized;

        lookInput.x = Input.GetAxisRaw("Mouse X");
        lookInput.y = Input.GetAxisRaw("Mouse Y");
    }

    [Header("Sway")]
    public float step = 0.01f;
    public float maxStepDistance = 0.06f;
    Vector3 swayPos;

    void Sway()
    {
        if (!sway) { swayPos = Vector3.zero; return; }

        Vector3 invertLook = lookInput * -step;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

        swayPos = invertLook;
    }

    [Header("Sway Rotation")]
    public float rotationStep = 4f;
    public float maxRotationStep = 5f;
    Vector3 swayEulerRot;

    void SwayRotation()
    {
        if (!swayRotation) { swayEulerRot = Vector3.zero; return; }

        Vector2 invertLook = lookInput * -rotationStep;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep);

        swayEulerRot = new Vector3(invertLook.y, -invertLook.x, -invertLook.x);
    }

    float smooth = 10f;
    float smoothRot = 12f;

    void CompositePositionRotation()
    {
        //position
        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPos + bobPosition, Time.deltaTime * smooth);

        //rotation
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(swayEulerRot) * Quaternion.Euler(bobEulerRotation), Time.deltaTime * smoothRot);
    }

    [Header("Bobbing")]
    public float speedCurve;
    float curveSin { get => Mathf.Sin(speedCurve); }
    float curveCos { get => Mathf.Cos(speedCurve); }

    public Vector3 travelLimit = Vector3.one * 0.025f;
    public Vector3 bobLimit = Vector3.one * 0.01f;
    Vector3 bobPosition;

    void BobOffset()
    {
        speedCurve += Time.deltaTime * ((mover.grounded ? rb.linearVelocity.magnitude : 1f) + swaySpeed);

        if (!bobOffset) { bobPosition = Vector3.zero; return; }

        bobPosition.x = (curveCos * bobLimit.x * (mover.grounded ? 1 : 0)) - (walkInput.x * travelLimit.x);
        bobPosition.y = (curveSin * bobLimit.y) - (rb.linearVelocity.y * travelLimit.y);
        bobPosition.z = -(walkInput.y * travelLimit.z);
    }

    [Header("Bob Rotation")]
    public Vector3 multiplier;
    Vector3 bobEulerRotation;

    void BobRotation()
    {
        if (!bobSway) { bobEulerRotation = Vector3.zero; return; }

        bobEulerRotation.x = (walkInput != Vector2.zero ? multiplier.x * (Mathf.Sin(2 * speedCurve)) : multiplier.x * (Mathf.Sin(2 * speedCurve) / 2));
        bobEulerRotation.y = (walkInput != Vector2.zero ? multiplier.y * curveCos : 0);
        bobEulerRotation.z = (walkInput != Vector2.zero ? multiplier.z * curveCos * walkInput.x : 0);
    }
}