using System.Security.Cryptography;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyCharacter : EnemyParameters
{
    [Header("UI")]
    public Slider enemyHealthSlider;

    [Header("Enemy Status")]
    public EnemyType type;
    public string currentStratergyName = "Idle";
    public Transform attackPoint;
    
    [Header("Attack Settings")]
    public float attackRange = 1f;
    public bool Cooldown = false;
    private float cooldownTime = 1.2f;
    public int BaseDamage = 5;

    NavMeshAgent agent;
    EnemyMovement movement;

    void Start()
    {
        Health = 100; // Example health value
        SetFightStrategy(new EnemieAttack());
        agent = GetComponent<NavMeshAgent>();
        movement = GetComponent<EnemyMovement>();
        if (type == EnemyType.Archer)
        {
            agent.stoppingDistance = 12f; // Maintain distance for archers
        }
    }
    public IFightStrategy CurrentFightStrategy { get; set; }
    public void SetFightStrategy(IFightStrategy strategy)
    {
        CurrentFightStrategy = strategy;
        Debug.Log($"{currentStratergyName} switched to {strategy.strategyName}!");
        currentStratergyName = strategy.strategyName;
    }

    // Update is called once per frame
    void Update()
    {


        if (cooldownTime > 0)
        {
            Cooldown = true;
            cooldownTime -= Time.deltaTime;
        }
        else
        {
            Cooldown = false;
            cooldownTime = 1.2f; // Reset cooldown time
        }
        if (Health <= 0)
        {
            Debug.Log($"{Name} has been defeated!");
            Destroy(gameObject);
        }
        if (movement.playerInRange && !Cooldown)
        {
            if (type == EnemyType.Archer)
            {
                var playerObj = movement.Player;
                if (playerObj == null) return;

                Vector3 toPlayer = playerObj.transform.position - transform.position;
                float dist = toPlayer.magnitude;
                float desiredDistance = Mathf.Max(agent.stoppingDistance, 12f); // ideal fighting distance
                float tooCloseThreshold = 9f; // if player is closer than this, run back

                // If player is too close -> run back until at desiredDistance, then go on cooldown
                if (dist < tooCloseThreshold)
                {
                    // Try physics-based evasion if Rigidbody is present
                    float evadeTime = 0.6f;
                    Rigidbody rb = GetComponent<Rigidbody>();
                    Vector3 away = (transform.position - playerObj.transform.position).normalized;

                    if (rb != null)
                    {
                        // Give physics control temporarily so AddForce actually moves the enemy
                        agent.isStopped = true;
                        agent.updatePosition = false;
                        rb.isKinematic = false;
                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;

                        // Tweak force to taste (direction away + small lift)
                        rb.AddForce(away * 8f + Vector3.up * 2f, ForceMode.Impulse);

                        // After evadeTime, StopKnockback will re-enable the agent and warp it to the physics position
                        StartCoroutine(StopKnockback(rb, agent, evadeTime));
                    }
                    else
                    {
                        // Fallback to NavMesh repositioning
                        agent.stoppingDistance = 0f; // disable stopping distance when retreating
                        Debug.Log("Player is too close! Retreating (navmesh fallback)...");
                        Vector3 desiredPos = playerObj.transform.position + away * desiredDistance;

                        if (NavMesh.SamplePosition(desiredPos, out NavMeshHit navHit, Mathf.Max(6.0f, desiredDistance + 2f), NavMesh.AllAreas))
                        {
                            agent.isStopped = false;
                            agent.SetDestination(navHit.position);
                        }
                        else
                        {
                            agent.isStopped = false;
                            agent.SetDestination(desiredPos);
                        }
                    }

                    cooldownTime = 1.2f;
                    Cooldown = true;
                    agent.stoppingDistance = 12f; // reset stopping distance
                    return; // handled repositioning, skip generic Attack call
                }

                // If too far -> move closer until stoppingDistance (12f)
                if (dist > desiredDistance + 0.5f)
                {
                    Debug.Log("Player is too far! Closing in...");
                    agent.SetDestination(playerObj.transform.position);
                    return; // keep closing in, skip attack for now
                }

                // In the sweet spot (about 11-12f): check front & line of sight, then attack and enter cooldown
                Vector3 dir = toPlayer.normalized;
                bool inFront = Vector3.Dot(transform.forward, dir) > 0.5f; // ~60deg cone
                RaycastHit hit;
                Vector3 rayOrigin = attackPoint != null ? attackPoint.position : transform.position;
                bool lineOfSight = Physics.Raycast(rayOrigin, dir, out hit, desiredDistance + attackRange) && hit.collider != null && hit.collider.gameObject == playerObj;

                if (inFront && lineOfSight)
                {
                    Debug.Log("Archer attacking the player!");
                    CurrentFightStrategy?.ExecuteAttack(playerObj.GetComponent<PlayerCharacter>(), this);
                    cooldownTime = 1.2f;
                    Cooldown = true;
                    return; // handled archer attack, skip generic Attack call
                }

                // If not in front / no LOS, you can optionally rotate/look or nudge position.
                // Let default flow continue for non-archer logic if needed.
            }
            Attack(movement.Player.GetComponent<PlayerCharacter>());
            Debug.Log($"{Name} is attacking the player!");
        }


    }
    
    private IEnumerator StopKnockback(Rigidbody rb, NavMeshAgent agent, float evadeTime)
    {
        yield return new WaitForSeconds(evadeTime);
        rb.velocity = Vector3.zero;
        agent.isStopped = false;
    }

    public void Attack(PlayerCharacter target)
    {
        if (target == null) return;

        // Default behaviour for other enemy types
        CurrentFightStrategy?.ExecuteAttack(target, this);
    }

    public void HitHandler(int damage)
    {
        Debug.Log($"{Name} hit for {damage} damage!");
        Health -= damage;
        CurrentFightStrategy?.ExecuteBlock(null, this);
    }
    
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
