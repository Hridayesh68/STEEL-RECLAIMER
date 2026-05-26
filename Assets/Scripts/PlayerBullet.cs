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
            other.GetComponentInParent<EnemyAi>();

        MeleeEnemyAI meleeEnemy =
            other.GetComponentInParent<MeleeEnemyAI>();

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