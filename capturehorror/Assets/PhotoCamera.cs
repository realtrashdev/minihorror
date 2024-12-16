using Unity.Cinemachine;
using UnityEngine;

public class PhotoCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera screen;
    [SerializeField] Light flash;

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
    }

    void Update()
    {
        Timer();
        GetInput();
        Lerp();
    }

    void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //click sound

            if (cooldown <= 0)
            {
                //shutter sound
                TakePicture();
            }
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            ZoomCamera();
        }
    }

    void Lerp()
    {
        flash.intensity = Mathf.Lerp(flash.intensity, 0, flashFadeSpeed * Time.deltaTime);
        screen.fieldOfView = Mathf.Lerp(screen.fieldOfView, zoom, zoomSpeed * Time.deltaTime);
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
        cooldown = flashCooldown;
    }

    void ZoomCamera()
    {
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
}
