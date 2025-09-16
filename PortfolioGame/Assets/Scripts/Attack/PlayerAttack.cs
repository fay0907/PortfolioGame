using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 10;
    public float attackRange = 1f;
    public Transform attackPoint;
    public LayerMask Enemy;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Player Attack");
            Attack();
        }
    }

    void Attack()
    {
        // Play attack animation here if you have one

        // Detect enemies in range of attack
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, Enemy);
        Debug.Log("Hit " + hitEnemies.Length + " enemies");
        // Damage them
        foreach (Collider enemy in hitEnemies)
        {
            enemy.GetComponent<EnemieHealth>().TakeDamage(attackDamage);
        }
    }

  

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
