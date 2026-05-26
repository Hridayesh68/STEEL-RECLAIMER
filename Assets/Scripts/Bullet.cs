using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 20;
    public float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit: " + other.name);

        // Ignore enemy bullets hitting enemy
        if (other.CompareTag("Enemy"))
            return;

        PlayerHealth player =
            other.GetComponent<PlayerHealth>();

        // Also check parent
        if (player == null)
        {
            player =
                other.GetComponentInParent<PlayerHealth>();
        }

        if (player != null)
        {
            Debug.Log("PLAYER HIT");

            player.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}