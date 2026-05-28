using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public int damage = 25;

    private void Start()
    {
        Destroy(gameObject, 3f);

        IgnorePlayerCollision();
    }

    private void IgnorePlayerCollision()
    {
        Collider bulletCollider = GetComponent<Collider>();

        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player == null) return;

        Collider[] playerColliders =
            player.GetComponentsInChildren<Collider>();

        foreach (Collider col in playerColliders)
        {
            Physics.IgnoreCollision(
                bulletCollider,
                col
            );
        }
    }

   private void OnTriggerEnter(Collider other)
{
    Debug.Log("Bullet hit: " + other.name);

    // Ignore triggers
    if (other.isTrigger)
        return;

    // Ignore player
    if (other.CompareTag("Player"))
        return;

    // Ignore weapon
    if (other.CompareTag("Weapon"))
        return;

    EnemyAi rangedEnemy =
        other.GetComponentInParent<EnemyAi>();

    if (rangedEnemy != null)
    {
        rangedEnemy.TakeDamage(damage);

        Destroy(gameObject);
        return;
    }

    MeleeEnemyAI meleeEnemy =
        other.GetComponentInParent<MeleeEnemyAI>();

    if (meleeEnemy != null)
    {
        meleeEnemy.TakeDamage(damage);

        Destroy(gameObject);
        return;
    }

    // Destroy on environment hit
    Destroy(gameObject);
}
}