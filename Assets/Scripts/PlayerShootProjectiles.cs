using UnityEngine;

public class PlayerShootProjectiles : MonoBehaviour
{
    [SerializeField] private GameObject pfBullet;
    [SerializeField] private CharacterAim_Base characterAim;

    [SerializeField] private float bulletSpeed = 40f;

    private void Awake()
    {
        if (characterAim != null)
        {
            characterAim.onShoot += PlayerShootProjectiles_OnShoot;
        }
        else
        {
            Debug.LogError("CharacterAim_Base reference missing!");
        }
    }

    private void OnDestroy()
    {
        if (characterAim != null)
        {
            characterAim.onShoot -= PlayerShootProjectiles_OnShoot;
        }
    }

    private void PlayerShootProjectiles_OnShoot(
        object sender,
        CharacterAim_Base.OnShootEventArgs e)
    {
        if (pfBullet == null)
        {
            Debug.LogError("Bullet prefab not assigned!");
            return;
        }

        GameObject bullet = Instantiate(
            pfBullet,
            e.gunEndPointPosition,
            Quaternion.LookRotation(e.shootDirection)
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity =
                e.shootDirection * bulletSpeed;
        }
        else
        {
            Debug.LogError("Bullet prefab missing Rigidbody!");
        }

        // Ignore collision with player
        Collider bulletCollider =
            bullet.GetComponent<Collider>();

        Collider playerCollider =
            GetComponentInParent<Collider>();

        if (bulletCollider != null &&
            playerCollider != null)
        {
            Physics.IgnoreCollision(
                bulletCollider,
                playerCollider
            );
        }
    }
}