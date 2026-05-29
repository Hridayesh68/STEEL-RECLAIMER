using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damage = 25;

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            return;

        Debug.Log("Bullet hit: " + other.name);

        EnemyHealth enemyHealth =
            other.GetComponentInParent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}