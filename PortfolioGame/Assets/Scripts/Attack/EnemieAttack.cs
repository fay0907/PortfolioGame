using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieAttack : MonoBehaviour
{
    public int attackDamage = 10;
    public float attackRange = 1f;
    public Transform attackPoint;
    public LayerMask Player;
    public float attackRate = 1f;
    private float nextAttackTime = 0f;

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, Player);
            if (hitPlayer.Length > 0)
            {
                Attack(hitPlayer[0]);
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack(Collider2D player)
    {
        // Play attack animation here if you have one

        // Damage the player
        player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
