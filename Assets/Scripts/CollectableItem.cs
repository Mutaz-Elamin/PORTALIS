using UnityEngine;
using UnityEngine.UI;

public class CollectableItem : MonoBehaviour, IInteractable
{
    private static int itemCount = 0;
    private const int maxItems = 5;
    private bool collected = false;

    public Text messageText; // assign a UI Text in Inspector

    public void Interact()
    {
        if (collected) return;

        if (itemCount < maxItems)
        {
            itemCount++;
            collected = true;
            if (messageText) messageText.text = "Item collected!";
            gameObject.SetActive(false);
        }
        else
        {
            if (messageText) messageText.text = "Inventory full!";
        }
    }
}
