using UnityEngine;

namespace GameCore
{
    [System.Serializable]
    public class AttackWave
    {
        [SerializeField] private float startTimeSecond;
        [SerializeField] private int maxSpawnCount;
        [SerializeField] private float spawnIntervalSecond;
        
        private float currentTimer;
        public int GetCurrentSpawnCount { get; private set; }
        public bool CanSpawnNext => GetCurrentSpawnCount < MaxSpawnCount;
        public float StartTimeSecond => startTimeSecond;
        public int MaxSpawnCount => maxSpawnCount;
        public float SpawnIntervalSecond => spawnIntervalSecond;

        public AttackWave(int maxSpawnCount, float spawnIntervalSecond, float startTimeSecond = 0)
        {
            this.startTimeSecond = startTimeSecond;
            this.maxSpawnCount = maxSpawnCount;
            this.spawnIntervalSecond = spawnIntervalSecond;
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