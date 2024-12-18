using UnityEngine;

public class CameraInteractible : MonoBehaviour
{
    public enum InteractionType { Reveal, Kill, Switch }

    [SerializeField] private InteractionType interactType;
    bool playerEntered;

    public void CameraInteract()
    {
        if (playerEntered)
        {
            Debug.Log("Can't interact, you're standing inside the object");
            return;
        }

        switch (interactType)
        {
            case InteractionType.Reveal:
                RevealObject();
                break;
            case InteractionType.Kill:
                KillObject();
                break;
            case InteractionType.Switch:
                SwitchObject();
                break;
        }
    }

    void RevealObject()
    {
        gameObject.layer = 0;
    }

    void KillObject()
    {
        gameObject.SetActive(false);
    }

    void SwitchObject()
    {
        switch (gameObject.layer)
        {
            case 8:
                gameObject.layer = 7;
                break;
            case 7:
                gameObject.layer = 8;
                break;
        }

        if (!gameObject.GetComponent<Collider>())
        {
            return;
        }

        switch (gameObject.layer)
        {
            case 8:
                gameObject.GetComponent<Collider>().isTrigger = false;
                break;
            case 7:
                gameObject.GetComponent<Collider>().isTrigger = true;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEntered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEntered = false;
        }
    }
}
