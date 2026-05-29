using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 20;
    public float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log("Hit: " + other.name);

    //     PlayerHealth player =
    //         other.GetComponentInParent<PlayerHealth>();

    // Debug.Log(player);
    //     if (player != null)
    //     {
    //         player.TakeDamage(damage);
    //     }

    //     Destroy(gameObject);
    // }
}