using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore
{
    [System.Serializable]
    public class AttackWave
    {
        private float currentTimer;
        private readonly List<IMonsterSetting> spawnMonsterOrderList;
        public MonsterMovementPath GetAttackPath { get; }
        public bool CanSpawnNext => GetCurrentSpawnCount < MaxSpawnCount;
        public float StartTimeSecond { get; }
        public IMonsterSetting GetCurrentSpawnMonsterSetting => spawnMonsterOrderList[GetCurrentSpawnCount];
        private int GetCurrentSpawnCount { get; set; }
        private int MaxSpawnCount { get; }
        private float SpawnIntervalSecond { get; }

        public AttackWave(float spawnIntervalSecond, List<IMonsterSetting> spawnMonsterOrderList, float startTimeSecond = 0, List<Vector2> pathPointList = null)
        {
            StartTimeSecond = startTimeSecond;
            MaxSpawnCount = spawnMonsterOrderList.Count;
            SpawnIntervalSecond = spawnIntervalSecond;
            this.spawnMonsterOrderList = spawnMonsterOrderList;
            GetAttackPath = ConvertMovementPathInfo(pathPointList);
            GetCurrentSpawnCount = 0;
        }

        public void AddSpawnCount(int addCount)
        {
            GetCurrentSpawnCount += addCount;
            if (GetCurrentSpawnCount > MaxSpawnCount)
                GetCurrentSpawnCount = MaxSpawnCount;
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