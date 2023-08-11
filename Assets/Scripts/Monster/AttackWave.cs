namespace GameCore
{
    public class AttackWave
    {
        public int GetMaxSpawnCount { get; }
        public int SpawnInterval { get; }
        public int GetCurrentSpawnCount { get; private set; }

        public AttackWave(int maxSpawnCount, int spawnInterval)
        {
            GetMaxSpawnCount = maxSpawnCount;
            SpawnInterval = spawnInterval;
        }

        public void AddSpawnCount(int addCount)
        {
            GetCurrentSpawnCount += addCount;
            if (GetCurrentSpawnCount > GetMaxSpawnCount)
                GetCurrentSpawnCount = GetMaxSpawnCount;
        }
    }
}