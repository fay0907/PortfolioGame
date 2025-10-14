using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieAttack : IFightStrategy
{

    public string strategyName { get; } = "Enemy Attack";
    public float attackRange = 0.5f;
    public int BaseDamage = 15;

    
    public void ExecuteAttack(PlayerCharacter target, EnemyCharacter attacker)
    {
        if (attacker.type == EnemyParameters.EnemyType.Beast)
        {
            BaseDamage = 25; // Beast enemies deal slightly less damage with melee attacks
        }
        else if (attacker.type == EnemyParameters.EnemyType.Humanoid)
        {
            BaseDamage = 20; // Humanoid enemies deal extra damage  with melee attacks
        }
        else if (attacker.type == EnemyParameters.EnemyType.Undead)
        {
            BaseDamage = 13; // Undead enemies deal extra damage with melee attacks
        }

        int damage = BaseDamage + UnityEngine.Random.Range(1, 5); // Base damage plus a random roll between 1 and 5

        if (target.CurrentFightStrategy is Block)
        {
            damage = Mathf.Max(0, damage - 3); // Blocking reduces damage by 3, but not below 0
            Debug.Log($"Enemy attacks {target.name} for {damage} damage, but the attack was blocked!");
        }
        else
        {
            Debug.Log($"Enemy attacks {target.name} for {damage} damage!");
            target.HitHandler(damage);
            EvasionStart(attacker, 0.2f);
        }
    }

    public void ExecuteBlock(PlayerCharacter Attacker, EnemyCharacter defender)
    {
        EvasionStart(defender, 1);
        Debug.Log("Enemy cannot block!");
    }

    private void EvasionStart(EnemyCharacter defender, float evadeTime)
    {
        Rigidbody rb = defender.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.back * 20, ForceMode.Impulse);
            defender.StartCoroutine(StopKnockback(rb, evadeTime));
        }
    }

    private IEnumerator StopKnockback(Rigidbody rb, float evadeTime)
    {
        yield return new WaitForSeconds(evadeTime);
        rb.velocity = Vector3.zero;
    }
    
}
