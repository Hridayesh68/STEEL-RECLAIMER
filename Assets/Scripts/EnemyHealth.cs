using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public int scoreValue = 10;

    [Header("Death")]
    public float deathHeight = -10f;

    [Header("UI")]
    public Slider healthBar;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip deathSound;

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    private void Update()
    {
        if (!isDead && transform.position.y < deathHeight)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        // Play hit sound
        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        Debug.Log($"{gameObject.name} took {damage} damage");
        Debug.Log($"Enemy Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log($"{gameObject.name} Died");

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(scoreValue);
        }

        // Hide enemy visuals and collider immediately
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            rend.enabled = false;
        }

        // Play death sound and keep it alive
        if (audioSource != null && deathSound != null)
        {
            audioSource.Stop();

            GameObject soundObject = new GameObject("EnemyDeathSound");
            AudioSource tempSource = soundObject.AddComponent<AudioSource>();

            tempSource.clip = deathSound;
            tempSource.volume = audioSource.volume;
            tempSource.pitch = audioSource.pitch;
            tempSource.spatialBlend = 0f; // 2D sound
            tempSource.Play();

            Destroy(soundObject, deathSound.length);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}