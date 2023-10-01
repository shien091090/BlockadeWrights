using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class AttackWave
    {
        private float currentTimer;
        public MonsterMovementPath GetAttackPath { get; }
        public bool CanSpawnNext => GetCurrentSpawnCount < MaxSpawnCount;
        public float StartTimeSecond { get; }
        private int GetCurrentSpawnCount { get; set; }
        private int MaxSpawnCount { get; }
        private float SpawnIntervalSecond { get; }

        public AttackWave(int maxSpawnCount, float spawnIntervalSecond, float startTimeSecond = 0, List<Vector2> pathPointList = null)
        {
            StartTimeSecond = startTimeSecond;
            MaxSpawnCount = maxSpawnCount;
            SpawnIntervalSecond = spawnIntervalSecond;
            GetAttackPath = ConvertMovementPathInfo(pathPointList);
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

        private MonsterMovementPath ConvertMovementPathInfo(List<Vector2> pathPointList)
        {
            MonsterMovementPath pathInfo = new MonsterMovementPath();
            if (pathPointList == null)
                return pathInfo;

            foreach (Vector2 pos in pathPointList)
            {
                pathInfo.AddPoint(pos);
            }

            return pathInfo;
        }
    }
}