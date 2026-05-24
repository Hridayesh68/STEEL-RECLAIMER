using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    public Transform firePoint;

    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    [Header("Health")]
    public float health = 100f;

    [Header("Patrol")]
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 10f;

    [Header("Attack")]
    public float timeBetweenAttacks = 2f;
    bool alreadyAttacked;
    public GameObject projectile;

    [Header("Ranges")]
    public float sightRange = 15f;
    public float attackRange = 10f;

    public bool playerInSightRange;
    public bool playerInAttackRange;

    private void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (player == null) return;

        // Detect player
        playerInSightRange =
            Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        playerInAttackRange =
            Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // State machine
        if (!playerInSightRange && !playerInAttackRange)
            Patroling();

        else if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();

        else if (playerInSightRange && playerInAttackRange)
            AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet)
            SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint =
            transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
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

        if (Physics.Raycast(walkPoint, Vector3.down, 2f, whatIsGround))
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
        // Stop moving
        agent.SetDestination(transform.position);

        // Aim at player chest
        Vector3 lookPos =
            player.position + Vector3.up * 1.5f;

        transform.LookAt(lookPos);

        if (!alreadyAttacked)
        {
            GameObject bullet =
                Instantiate(projectile,
                firePoint.position,
                firePoint.rotation);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            Vector3 shootDirection =
                (lookPos - firePoint.position).normalized;

            rb.linearVelocity = Vector3.zero;

            rb.AddForce(shootDirection * 35f,
                ForceMode.Impulse);

            alreadyAttacked = true;

            Invoke(nameof(ResetAttack),
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
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}