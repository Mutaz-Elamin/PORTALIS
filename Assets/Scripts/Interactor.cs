using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform interactorSource;
    public float interactorRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(interactorSource.position, interactorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactorRange))
            {
               if (hitInfo.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
               {
                   interactable.Interact();
                }
            }
        }
    }
}
