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
        EnemyAi rangedEnemy =
            other.GetComponent<EnemyAi>();

        MeleeEnemyAI meleeEnemy =
            other.GetComponent<MeleeEnemyAI>();

        if (rangedEnemy != null)
        {
            rangedEnemy.TakeDamage(damage);
        }

        if (meleeEnemy != null)
        {
            meleeEnemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}