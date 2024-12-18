using Unity.Cinemachine;
using UnityEngine;

public class PhotoCamera : MonoBehaviour
{
    public bool aiming = false;
    public bool hitWall = false;

    [Header("References")]
    [SerializeField] Camera screen;
    [SerializeField] Light flash;
    [SerializeField] SliderScript batterySlider;
    [SerializeField] PlayerMovement playerMovement;
    ItemManager itemManager;

    [Header("Stats")]
    public int battery;

    [Header("Positioning")]
    [SerializeField] Vector3 holdPos;
    [SerializeField] Vector3 aimPos;
    [SerializeField] Vector3 sprintPos;
    [SerializeField] Vector3 wallHitPos;

    [Header("Aiming")]
    [SerializeField] float aimSpeed;

    [Header("Flash Settings")]
    [SerializeField] float flashIntensity;
    [SerializeField] float flashFadeSpeed;
    [SerializeField] float flashCooldown;
    float cooldown;

    [Header("Zoom Settings")]
    [SerializeField] float maxZoom;
    [SerializeField] float minZoom;
    [SerializeField] float zoomSpeed;
    [SerializeField] float zoomIncrement;
    float zoom;

    void Start()
    {
        zoom = screen.fieldOfView;
        itemManager = GetComponentInParent<ItemManager>();
    }

    void Update()
    {
        Timer();
        GetInput();
        CheckForWall();

        if (itemManager.switching)
        {
            aiming = false;
        }

        Lerp();
    }

    void GetInput()
    {
        if (Input.GetMouseButtonDown(0) && aiming)
        {
            //click sound

            if (cooldown <= 0 && battery > 0)
            {
                //shutter sound
                TakePicture();
            }
        }

        switch (Input.GetMouseButton(1))
        {
            case true:
                aiming = true;
                break;
            case false: 
                aiming = false;
                break;
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            ZoomCamera();
        }
    }

    void CheckForWall()
    {
        Camera cam = Camera.main;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, 1.4f))
        {
            if (hitInfo.collider.isTrigger)
            {
                return;
            }

            hitWall = true;
            aiming = false;
        }

        else if (Physics.Raycast(playerMovement.gameObject.transform.position, playerMovement.gameObject.transform.forward, 1.4f))
        {
            if (hitInfo.collider.isTrigger)
            {
                return;
            }

            hitWall = true;
            aiming = false;
        }

        else
        {
            hitWall = false;
        }

        Debug.DrawRay(cam.transform.position, cam.transform.forward, Color.green);
        Debug.DrawRay(playerMovement.gameObject.transform.position, playerMovement.gameObject.transform.forward, Color.blue);
    }

    void Lerp()
    {
        flash.intensity = Mathf.Lerp(flash.intensity, 0, flashFadeSpeed * Time.deltaTime);
        screen.fieldOfView = Mathf.Lerp(screen.fieldOfView, zoom, zoomSpeed * Time.deltaTime);

        Vector3 position;
        Vector3 rotation;

        if (hitWall)
        {
            position = wallHitPos;
            rotation = new Vector3(0, 0, 0);
        }

        else if (aiming)
        {
            position = aimPos;
            rotation = new Vector3(0, 0, 0);
        }

        else if (playerMovement.sprinting)
        {
            position = sprintPos;
            rotation = new Vector3(0, 0, 5);
        }

        else
        {
            position = holdPos;
            rotation = new Vector3(0, 0, 5);
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, position, aimSpeed * Time.deltaTime);
        transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, rotation, aimSpeed * Time.deltaTime);

        if (!aiming)
        {
            zoom = Mathf.Lerp(zoom, minZoom, zoomSpeed * Time.deltaTime);
        }
    }

    void Timer()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }

    void TakePicture()
    {
        flash.intensity = flashIntensity;
        battery -= 1;
        batterySlider.UpdateValue(battery);
        CheckForInteraction();
        cooldown = flashCooldown;
    }

    void ZoomCamera()
    {
        if (!aiming)
        {
            return;
        }

        zoom += -Input.mouseScrollDelta.y * zoomIncrement;

        if (zoom < maxZoom)
        {
            zoom = maxZoom;
        }

        else if (zoom > minZoom)
        {
            zoom = minZoom;
        }
    }

    void CheckForInteraction()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 20f))
        {
            if (hitInfo.collider.gameObject.GetComponent<CameraInteractible>())
            {
                hitInfo.collider.gameObject.GetComponent<CameraInteractible>().CameraInteract();
            }
        }
    }
}
