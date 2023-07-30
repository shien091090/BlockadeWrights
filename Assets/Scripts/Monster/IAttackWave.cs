namespace GameCore
{
    public interface IAttackWave
    {
        int GetMaxSpawnCount { get; }
        int GetCurrentSpawnCount { get; }
    }
}