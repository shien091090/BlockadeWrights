namespace GameCore
{
    public interface IBuildingAttackView
    {
        void StartAttack(IAttackTarget attackTarget, float attackPower);
    }
}