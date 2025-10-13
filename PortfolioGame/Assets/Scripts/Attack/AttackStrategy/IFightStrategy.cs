public interface IFightStrategy
{
    void ExecuteAttack(PlayerCharacter attacker, EnemyCharacter target);
    string strategyName { get; }
}
