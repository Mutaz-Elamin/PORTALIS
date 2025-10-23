using UnityEngine;
using UnityEngine.UI;

public class CollectableItem : MonoBehaviour, IInteractable
{
    private static int itemCount = 0;
    private const int maxItems = 10;
    private bool collected = false;

    public Text messageText; // assign a UI Text in Inspector

    public void Interact()
    {
        if (collected) return;


        collected = true;

        gameObject.SetActive(false);
        

    }
}
