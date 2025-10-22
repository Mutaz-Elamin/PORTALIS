using UnityEngine;
using UnityEngine.UI;

public class HungerSystem : MonoBehaviour
{
    public float maxHunger = 100f;
    [HideInInspector] public float currentHunger; 
    
    public Slider hungerSlider;
    
    public float walkDepletionRate = 0.05f; 
    public float sprintDepletionRate = 0.15f; 
    public float passiveDepletionAmount = 1f;
    public float passiveDepletionInterval = 10f; 

    private float passiveTimer = 0f;

    void Start()
    {
        currentHunger = maxHunger;
        
        if (hungerSlider != null)
        {
            hungerSlider.maxValue = maxHunger;
        }

        UpdateHungerUI();
    }

    void Update()
    {
        passiveTimer += Time.deltaTime;
        if (passiveTimer >= passiveDepletionInterval)
        {
            DepleteHunger(passiveDepletionAmount);
            passiveTimer = 0f; 
        }

        if (currentHunger <= 0)
        {
            // Starvation damage goes here...
        }
    }

    public void DepleteHunger(float amount)
    {
        currentHunger -= amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        UpdateHungerUI();
    }

    public void GainHunger(float amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        UpdateHungerUI();
    }

    private void UpdateHungerUI()
    {
        if (hungerSlider != null)
        {
            hungerSlider.value = currentHunger;
        }
    }
}