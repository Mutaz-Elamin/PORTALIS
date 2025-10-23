using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthSlider;
    public CanvasGroup gameOverPanel;

    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();

        currentHealth = maxHealth;
        
        healthSlider.maxValue = maxHealth;
        UpdateHealthBarUI();
        
        Time.timeScale = 1f; 
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0);

        UpdateHealthBarUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Heal(int healAmount)
    {
        if (isDead) return;
        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        UpdateHealthBarUI();
    }

    void UpdateHealthBarUI()
    {
        healthSlider.value = currentHealth;
    }

    void Die()
    {
        isDead = true;
        
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        CharacterController controller = GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        GameOver();
    }

    void GameOver()
    {
        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.alpha = 1f;
            gameOverPanel.interactable = true;
            gameOverPanel.blocksRaycasts = true;
            playerController.enabled = false;
            playerController.GetComponent<InputManager>().enabled = false;

        }
        RestartGame();
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}