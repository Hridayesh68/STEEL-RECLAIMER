using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Fall Death")]
    public float deathHeight = -10f;

    [Header("UI")]
    public Slider healthSlider;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip deathSound;

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        Debug.Log("Player Spawned With Health: " + currentHealth);
    }

    private void Update()
    {
        if (!isDead && transform.position.y < deathHeight)
        {
            Debug.Log("Player Fell Into Void");
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        UpdateHealthUI();

        // Play damage sound
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        Debug.Log("Player Took Damage: " + damage);
        Debug.Log("Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead)
            return;

        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log("Player Died");

        // Play death sound
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        if (UIManager.Instance != null)
        {
            // Delay Game Over so sound can play
            Invoke(nameof(ShowGameOver), 1f);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void ShowGameOver()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.GameOver();
        }

        gameObject.SetActive(false);
    }
}