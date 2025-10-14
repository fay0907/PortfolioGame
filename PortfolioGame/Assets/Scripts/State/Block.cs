using System;
using UnityEngine;

public class Block : IFightStrategy
{
    public string strategyName { get; } = "Block";

    public void ExecuteAttack(PlayerCharacter attacker, EnemyCharacter target)
    {
        attacker.SetFightStrategy(new SwordAttack());
        Debug.Log("You have switched to Sword Attack strategy!");
    }
    public void ExecuteBlock(PlayerCharacter defender, EnemyCharacter attacker)
    {
        Debug.Log("You are blocking the next attack!");
        
    }
}
