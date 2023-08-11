namespace GameCore
{
    public class AttackWave
    {
        private float currentTimer;
        public float StartTimeSecond { get; }
        public int GetMaxSpawnCount { get; }
        public float SpawnIntervalSecond { get; }
        public int GetCurrentSpawnCount { get; private set; }
        public bool CanSpawnNext => GetCurrentSpawnCount < GetMaxSpawnCount;

        public AttackWave(int maxSpawnCount, float spawnIntervalSecond, float startTimeSecond = 0)
        {
            StartTimeSecond = startTimeSecond;
            GetMaxSpawnCount = maxSpawnCount;
            SpawnIntervalSecond = spawnIntervalSecond;
        }

        public bool UpdateTimerAndCheckSpawn(float deltaTime)
        {
            currentTimer += deltaTime;
            
            if(currentTimer >= SpawnIntervalSecond)
            {
                currentTimer = 0;
                return true;
            }
            else
                return false;
        }

        public void AddSpawnCount(int addCount)
        {
            GetCurrentSpawnCount += addCount;
            if (GetCurrentSpawnCount > GetMaxSpawnCount)
                GetCurrentSpawnCount = GetMaxSpawnCount;
        }
    }
}