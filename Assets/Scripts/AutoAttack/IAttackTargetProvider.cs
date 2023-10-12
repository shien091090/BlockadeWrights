namespace GameCore
{
    public interface IAttackTargetProvider
    {
        public IAttackTarget GetAttackTarget { get; }
    }
}