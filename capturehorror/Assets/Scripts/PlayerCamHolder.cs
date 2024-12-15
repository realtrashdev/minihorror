using UnityEngine;

public class PlayerCamHolder : MonoBehaviour
{
    Transform playerTransform;
    
    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position;
    }
}
