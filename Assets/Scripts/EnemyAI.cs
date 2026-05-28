using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;
    public Transform firePoint;
    public GameObject projectile;

    [Header("Layers")]
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    [Header("Health")]
    public float health = 100f;

    [Header("Score")]
    public int scoreValue = 10;

    [Header("Patrol")]
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange = 10f;

    [Header("Attack")]
    public float timeBetweenAttacks = 2f;
    private bool alreadyAttacked;
    public float projectileForce = 35f;

    [Header("Ranges")]
    public float sightRange = 15f;
    public float attackRange = 10f;

    [Header("Detection")]
    public bool playerInSightRange;
    public bool playerInAttackRange;

    private bool isDead = false;

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
        if (player == null || isDead)
            return;

        // Detect player
        playerInSightRange =
            Physics.CheckSphere(
                transform.position,
                sightRange,
                whatIsPlayer
            );

        playerInAttackRange =
            Physics.CheckSphere(
                transform.position,
                attackRange,
                whatIsPlayer
            );

        // State Machine
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
        float randomX =
            Random.Range(-walkPointRange, walkPointRange);

        float randomZ =
            Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(
            transform.position.x + randomX,
            transform.position.y,
            transform.position.z + randomZ
        );

        if (Physics.Raycast(
            walkPoint + Vector3.up * 2f,
            Vector3.down,
            4f,
            whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);

        Vector3 lookPos =
            new Vector3(
                player.position.x,
                transform.position.y,
                player.position.z
            );

        transform.LookAt(lookPos);
    }

    private void AttackPlayer()
    {
        // Stop moving
        agent.SetDestination(transform.position);

        // Look at player
        Vector3 lookPos =
            new Vector3(
                player.position.x,
                transform.position.y,
                player.position.z
            );

        transform.LookAt(lookPos);

        if (!alreadyAttacked)
        {
            Shoot();

            alreadyAttacked = true;

            Invoke(nameof(ResetAttack),
                timeBetweenAttacks);
        }
    }

    private void Shoot()
    {
        if (projectile == null || firePoint == null)
            return;

        // Spawn bullet
        GameObject bullet =
            Instantiate(
                projectile,
                firePoint.position,
                firePoint.rotation
            );

        Rigidbody rb =
            bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 targetPoint =
                player.position + Vector3.up * 1.5f;

            Vector3 shootDirection =
                (targetPoint - firePoint.position).normalized;

            rb.linearVelocity = Vector3.zero;

            rb.AddForce(
                shootDirection * projectileForce,
                ForceMode.Impulse
            );
        }

        Destroy(bullet, 5f);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        // Stop enemy movement
        if (agent != null)
        {
            agent.isStopped = true;
        }

        // Add score
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(scoreValue);
        }

        // Destroy enemy
        Destroy(gameObject, 0.5f);
    }

    private void OnDrawGizmosSelected()
    {
        // Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );

        // Sight range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            sightRange
        );
    }
}