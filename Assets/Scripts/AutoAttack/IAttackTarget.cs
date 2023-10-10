namespace GameCore
{
    public interface IAttackTarget
    {
        ITransform GetTransform { get; }
        string Id { get; }
        bool IsDead { get; }
        bool IsGoingToDie { get; }
        void Damage(float damageValue);
        void PreDamage(float damageValue);
    }
}