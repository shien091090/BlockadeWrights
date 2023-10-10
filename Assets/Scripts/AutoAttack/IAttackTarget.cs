namespace GameCore
{
    public interface IAttackTarget
    {
        ITransform GetTransform { get; }
        string Id { get; }
        EntityState GetEntityState { get; }
        void Damage(float damageValue);
        void PreDamage(float damageValue);
    }
}