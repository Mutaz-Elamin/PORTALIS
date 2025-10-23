using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Define the IInteractable interface
interface IInteractable
{
    void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform interactorSource;
    public float interactorRange;
    private int count;
    private int numPickups = 10; 
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;
    public CanvasGroup gameOverPanel;
    private PlayerController playerController;

    
    void Start()
    {
        count = 0;
        winText.text = " ";
        SetCountText();

    }

    
    void Update()
    // Check for interaction input
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(interactorSource.position, interactorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, interactorRange))
            {
               if (hitInfo.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
               {
                   interactable.Interact();
                    count++;
                    SetCountText();


                }
            }
        }
    }
    // Update the score text and check for win condition
    private void SetCountText()
    {
         scoreText.text = " Score : " + count.ToString();
         if (count >= numPickups)
        {
            if (playerController != null)
            {
                playerController.enabled = false;
            }

            CharacterController controller = GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
            }
            winText.text = " You win ! ";
            gameOverPanel.alpha = 1f;
            gameOverPanel.interactable = true;
            gameOverPanel.blocksRaycasts = true;
            playerController.enabled = false;
            playerController.GetComponent<InputManager>().enabled = false;
        }
    }


}
