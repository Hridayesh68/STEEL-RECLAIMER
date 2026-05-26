using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;

    public int currentHealth;

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;

        Debug.Log("Player Spawned With Health: " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        // Prevent extra damage after death
        if (isDead)
            return;

        currentHealth -= damage;

        Debug.Log("Player Took Damage: " + damage);
        Debug.Log("Current Health: " + currentHealth);

        // Clamp health
        if (currentHealth <= 0)
        {
            currentHealth = 0;

            Die();
        }
    }

    private void Die()
    {
        // Prevent multiple deaths
        if (isDead)
            return;

        isDead = true;

        Debug.Log("PLAYER DIED");

        // Disable player controls
        gameObject.SetActive(false);

        // Show game over UI
        if (UIManager.Instance != null)
        {
            UIManager.Instance.GameOver();
        }
        else
        {
            Debug.LogError("UIManager Instance Missing!");
        }
    }
}