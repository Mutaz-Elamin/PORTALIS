using UnityEngine;
using UnityEngine.UI;

public class CollectableItem : MonoBehaviour, IInteractable
{
    
   
    private bool collected = false;

    public Text messageText;

    //Disable the item and update the count when interacted with
    public void Interact()
    {
        if (collected) return;


        collected = true;

        gameObject.SetActive(false);
        

    }
}
