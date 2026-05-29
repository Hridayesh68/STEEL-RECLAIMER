using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;

    [Header("Target")]
    public Transform player;

    [Header("Layers")]
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    [Header("Health")]
    public float health = 100f;

    [Header("Patrolling")]
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange = 10f;

    [Header("Attack")]
    public float timeBetweenAttacks = 2f;
    private bool alreadyAttacked;

    public GameObject projectile;
    public Transform firePoint;
    public float projectileSpeed = 25f;

    [Header("Ranges")]
    public float sightRange = 20f;
    public float attackRange = 10f;

    private bool playerInSightRange;
    private bool playerInAttackRange;

    private void Awake()
    {
        // Find PlayerArmature directly
        GameObject armature =
            GameObject.Find("PlayerArmature");

        if (armature != null)
        {
            player = armature.transform;
        }
        else
        {
            Debug.LogError("PlayerArmature not found!");
        }

        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (player == null)
            return;

        playerInSightRange =
            Physics.CheckSphere(
                transform.position,
                sightRange,
                whatIsPlayer);

        playerInAttackRange =
            Physics.CheckSphere(
                transform.position,
                attackRange,
                whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        else if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }
    }

    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint =
            transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ =
            Random.Range(-walkPointRange, walkPointRange);

        float randomX =
            Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(
            transform.position.x + randomX,
            transform.position.y,
            transform.position.z + randomZ
        );

        if (Physics.Raycast(
            walkPoint,
            Vector3.down,
            2f,
            whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        Vector3 lookPos = player.position;
        lookPos.y = transform.position.y;

        transform.LookAt(lookPos);

        if (!alreadyAttacked)
        {
            if (projectile != null &&
                firePoint != null)
            {
                GameObject bullet = Instantiate(
                    projectile,
                    firePoint.position,
                    Quaternion.identity
                );

                Rigidbody rb =
                    bullet.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    Vector3 shootDirection =
                        (player.position -
                         firePoint.position).normalized;

                    rb.linearVelocity =
                        shootDirection * projectileSpeed;
                }
            }

            alreadyAttacked = true;

            Invoke(
                nameof(ResetAttack),
                timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            sightRange);
    }
}