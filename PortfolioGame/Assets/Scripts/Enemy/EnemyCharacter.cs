using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCharacter : EnemyParameters
{
    public EnemyType type;
    public string currentStratergyName = "Idle";
    public int BaseDamage = 5;
    public Transform attackPoint;
    NavMeshAgent agent;
    EnemyMovement movement;
    public float attackRange = 1f;
    public bool Cooldown = false;
    private float cooldownTime = 1.2f;

    void Start()
    {
        Health = 100; // Example health value
        SetFightStrategy(new EnemieAttack());
        agent = GetComponent<NavMeshAgent>();
        movement = GetComponent<EnemyMovement>();
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
        if (movement.playerInRange && Vector3.Distance(transform.position, agent.destination) <= attackRange && !Cooldown)
        {
            Attack(movement.Player.GetComponent<PlayerCharacter>());
            Debug.Log($"{Name} is attacking the player!");
        }
    }

    public void Attack(PlayerCharacter target)
    {
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
