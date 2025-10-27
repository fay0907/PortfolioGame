using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowAttack : IFightStrategy
{
     string IFightStrategy.strategyName { get; } = "Bow Attack";
    public void ExecuteAttack(PlayerCharacter attacker, EnemyCharacter target)
    {
        int damage = attacker.BaseDamage + attacker.RollDamage(2, 6); // Base damage plus a random roll between 2 and 6
        // if (target.type == EnemyParameters.EnemyType.Flying) damage += 3; // Flying enemies take extra damage from bow attacks

        Debug.Log($"Bow Attack attacks {target.Name} with a bow for {damage} damage!");
        target.HitHandler(damage);
    }
    
    public void ExecuteBlock(PlayerCharacter defender, EnemyCharacter target)
    {
        defender.SetFightStrategy(new Block());
        Debug.Log("You have switched to Block strategy!");
    }

    // Update is called once per frame
    void Update() 
    {
        
    }
}
