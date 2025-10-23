using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

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
    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        winText.text = " ";
        SetCountText();

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
                    count++;
                    SetCountText();


                }
            }
        }
    }
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
