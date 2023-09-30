
namespace GameCore
{
    public interface IAttackTarget
    {
        ITransform GetTransform { get; }
        string Id { get; }
        bool IsDead { get; }
        void Damage(float damageValue);
    }

}