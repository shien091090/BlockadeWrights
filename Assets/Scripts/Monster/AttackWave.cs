using UnityEngine;

namespace GameCore
{
    public class AttackWave
    {
        private float currentTimer;
        private int GetCurrentSpawnCount { get; set; }
        public bool CanSpawnNext => GetCurrentSpawnCount < MaxSpawnCount;
        public float StartTimeSecond { get; }
        private int MaxSpawnCount { get; }
        private float SpawnIntervalSecond { get; }

        public AttackWave(int maxSpawnCount, float spawnIntervalSecond, float startTimeSecond = 0)
        {
            StartTimeSecond = startTimeSecond;
            MaxSpawnCount = maxSpawnCount;
            SpawnIntervalSecond = spawnIntervalSecond;
        }

        public bool UpdateTimerAndCheckSpawn(float deltaTime)
        {
            currentTimer += deltaTime;

            if (currentTimer >= SpawnIntervalSecond)
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
            if (GetCurrentSpawnCount > MaxSpawnCount)
                GetCurrentSpawnCount = MaxSpawnCount;
        }
    }
}