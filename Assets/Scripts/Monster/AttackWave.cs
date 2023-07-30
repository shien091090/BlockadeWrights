namespace GameCore
{
    public class AttackWave
    {
        public int GetMaxSpawnCount { get; }
        public int GetCurrentSpawnCount { get; private set; }

        public AttackWave(int maxSpawnCount)
        {
            GetMaxSpawnCount = maxSpawnCount;
        }

        public void AddSpawnCount(int addCount)
        {
            GetCurrentSpawnCount += addCount;
            if (GetCurrentSpawnCount > GetMaxSpawnCount)
                GetCurrentSpawnCount = GetMaxSpawnCount;
        }
    }
}