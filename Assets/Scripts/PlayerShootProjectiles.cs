using UnityEngine;

public class PlayerShootProjectiles : MonoBehaviour
{
    [SerializeField] private GameObject pfBullet;
    [SerializeField] private CharacterAim_Base characterAim;

    [SerializeField] private float bulletSpeed = 50f;

    private void Awake()
    {
        characterAim.onShoot += ShootProjectile;
    }

    private void OnDestroy()
    {
        characterAim.onShoot -= ShootProjectile;
    }

    private void ShootProjectile(
        object sender,
        CharacterAim_Base.OnShootEventArgs e)
    {
        GameObject bullet = Instantiate(
            pfBullet,
            e.spawnPosition,
            Quaternion.LookRotation(e.shootDirection)
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity =
                e.shootDirection * bulletSpeed;
        }
    }
}