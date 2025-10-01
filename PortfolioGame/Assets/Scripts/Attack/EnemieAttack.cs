using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieAttack : MonoBehaviour
{
    [Header("Attack settings")]
    public int attackDamage = 10;
    public float attackRange = 1f;
    public Transform attackPoint;
    public LayerMask Player; // stel in via Inspector (selecteer de Player-layer)
    public float attackRate = 1f; // aanvallen per seconde

    private float nextAttackTime = 0f;

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (attackPoint == null)
            {
                Debug.LogWarning("AttackPoint niet ingesteld op " + gameObject.name);
                return;
            }

            // 3D overlap sphere om spelers te detecteren
            Collider[] hitPlayers = Physics.OverlapSphere(attackPoint.position, attackRange, Player);
            if (hitPlayers.Length > 0)
            {
                Attack(hitPlayers[0]); // targeteer de eerste gevonde speler
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack(Collider target)
    {
        if (target == null) return;

        // speel animatie hier (optioneel)
        // Animator anim = GetComponent<Animator>();
        // if (anim) anim.SetTrigger("Attack");

        // probeer PlayerHealth component te vinden en schade toe te brengen
        PlayerHealth ph = target.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            ph.TakeDamage(attackDamage);
        }
        else
        {
            // als de PlayerHealth niet direct op de collider zit, probeer het op het parent object
            PlayerHealth phParent = target.GetComponentInParent<PlayerHealth>();
            if (phParent != null)
                phParent.TakeDamage(attackDamage);
            else
                Debug.LogWarning("Geen PlayerHealth component gevonden op target: " + target.name);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
