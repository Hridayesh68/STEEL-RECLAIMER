using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
public int scoreValue = 10;
private bool isDead = false;
    public float deathHeight = -10f;
    [Header("UI")]
    public Slider healthBar;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        Debug.Log(gameObject.name + " took " + damage + " damage");
        Debug.Log("Enemy Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
         if (transform.position.y < deathHeight)
        {
            Die();
        }
    }

   private void Die()
{
    if (isDead) return;
    isDead = true;

    Debug.Log(gameObject.name + " Died");

    if (ScoreManager.instance != null)
    {
        ScoreManager.instance.AddScore(scoreValue);
    }
    else
    {
        Debug.LogError("ScoreManager instance not found!");
    }

    Destroy(gameObject);
}
}