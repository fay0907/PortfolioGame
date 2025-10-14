public interface IFightStrategy
{
    void ExecuteAttack(PlayerCharacter attacker, EnemyCharacter target);
    void ExecuteBlock(PlayerCharacter defender, EnemyCharacter enemyDefender);
    string strategyName { get; }
}
