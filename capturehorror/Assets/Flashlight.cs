using UnityEngine;

public class Flashlight : MonoBehaviour
{
    Light spotLight;
    bool lightOn;

    float cooldown = 0;

    [Header("Stats")]
    public int battery;

    [Header("Positioning")]
    [SerializeField] Vector3 holdPos;
    [SerializeField] Vector3 sprintPos;
    [SerializeField] Vector3 wallHitPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spotLight = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        Timers();
        GetInput();
    }

    void Timers()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
    }

    void GetInput()
    {
        if (cooldown > 0)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.F))
        {
            lightOn = !lightOn;
            cooldown = 1;

            switch (lightOn)
            {
                case true:
                    spotLight.intensity = 50;
                    break;
                case false:
                    spotLight.intensity = 0;
                    break;
            }
        }
    }
}
