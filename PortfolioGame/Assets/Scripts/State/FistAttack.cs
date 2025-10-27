using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FistAttack : IFightStrategy
{
    string IFightStrategy.strategyName { get; } = "Fist Attack";
    public void ExecuteAttack(PlayerCharacter attacker, EnemyCharacter target)
    {
        int damage = attacker.BaseDamage + attacker.RollDamage(1, 4); // Base damage plus a random roll between 1 and 4
        if (target.type == EnemyParameters.EnemyType.Humanoid) damage += 1; // Humanoid enemies take extra damage from fist attacks

        Debug.Log($"Fist Attack attacks {target.Name} with fists for {damage} damage!");
        target.HitHandler(damage);
    }

    public void ExecuteBlock(PlayerCharacter defender, EnemyCharacter target)
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //
}