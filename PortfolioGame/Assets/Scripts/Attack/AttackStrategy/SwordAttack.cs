using System;
using UnityEngine;

public class SwordAttack : IFightStrategy
{
    public string strategyName { get; } = "Sword Attack";

    public void ExecuteAttack(PlayerCharacter attacker, EnemyCharacter target)
    {
        int damage = attacker.BaseDamage + attacker.RollDamage(3,7); // Base damage plus a random roll between 1 and 5
        if (target.type == EnemyParameters.EnemyType.Beast) damage += 2; // Armored enemies take extra damage from sword attacks
        
        Debug.Log($"{strategyName} attacks {target.Name} with a sword for {damage} damage!");
        target.Health -= damage;
    }
}
