using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    [Header("Player Status")]
    public string currentStratergyName = "Idle";

    [Header("Attack Settings")]
    public Transform attackPoint;
    public int BaseDamage = 10;
    public float attackRange = 1f;
    public LayerMask Enemy;
    public int health = 100;

    [Header("UI")]
    public Canvas gameOverCanvas;
    public Slider healthSlider;
    public GameObject HealthFilling;

    void Start()
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetFightStrategy(new SwordAttack());
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Block(null);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, Enemy);
            if (hitEnemies != null && hitEnemies.Length > 0)
            {
                Attack(hitEnemies[0].GetComponent<EnemyCharacter>());
            }
            hitEnemies = null;
        }
    }


    public void HitHandler(int damage)
    {
        Debug.Log($"Player hit for {damage} damage!");
        health -= damage;
        healthSlider.value = health;
        if (health <= 0)
        {
            Debug.Log("Player has been defeated!");
            HealthFilling.SetActive(false);
            gameOverCanvas.gameObject.SetActive(true);
        }
    }

    
    
    public int RollDamage(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }
    public IFightStrategy CurrentFightStrategy { get; set; }

    public void SetFightStrategy(IFightStrategy strategy)
    {
        CurrentFightStrategy = strategy;
        Debug.Log($"{currentStratergyName} switched to {strategy.strategyName}!");
        currentStratergyName = strategy.strategyName;
    }

    public void Attack(EnemyCharacter target)
    {
        CurrentFightStrategy?.ExecuteAttack(this, target);
    }
    public void Block(EnemyCharacter attacker)
    {
        CurrentFightStrategy?.ExecuteBlock(this, attacker);
    }
    
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
