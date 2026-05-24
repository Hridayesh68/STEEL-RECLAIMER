using UnityEngine;

public class PlayerShootProjectiles : MonoBehaviour
{
    [SerializeField] private GameObject pfBullet;
    [SerializeField] private CharacterAim_Base characterAim;

    [SerializeField] private float bulletSpeed = 25f;

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

        // Spawn bullet
        GameObject bullet = Instantiate(
            pfBullet,
            e.gunEndPointPosition,
            Quaternion.LookRotation(e.shootDirection)
        );

        // Get rigidbody
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Shoot bullet
            rb.linearVelocity =
                e.shootDirection.normalized * bulletSpeed;
        }
        else
        {
            Debug.LogError("Bullet prefab missing Rigidbody!");
        }
    }
}