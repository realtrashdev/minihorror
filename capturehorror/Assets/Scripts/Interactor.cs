using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

interface IInteractible
{
    public void Interact();

    public void SetInteractPrompt();
}

public class Interactor : MonoBehaviour
{
    public TMP_Text interactPrompt;

    public Transform interactorSource;
    public float interactRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray r = new Ray(interactorSource.position, interactorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractible interactObj))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactObj.Interact();
                }

                Debug.Log("Interactible Found");
                interactObj.SetInteractPrompt();
            }

            else
            {
                interactPrompt.text = "";
            }
        }

        else
        {
            interactPrompt.text = "";
        }
    }
}
