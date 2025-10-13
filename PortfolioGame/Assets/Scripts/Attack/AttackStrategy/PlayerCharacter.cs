using System;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public string currentStratergyName = "Idle";
    public int BaseDamage = 5;
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask Enemy;
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetFightStrategy(new SwordAttack());
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, Enemy);

            Attack(hitEnemies[0].GetComponent<EnemyCharacter>());
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
    
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
