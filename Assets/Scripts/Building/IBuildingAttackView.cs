namespace GameCore
{
    public interface IBuildingAttackView
    {
        void LaunchAttack(IAttackTarget attackTarget, float attackPower);
    }
}