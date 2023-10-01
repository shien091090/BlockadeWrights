using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore
{
    [System.Serializable]
    public class AttackWaveSetting
    {
        [SerializeField] private float startTimeSecond;
        [SerializeField] private float spawnIntervalSecond;
        [SerializeField] private List<Vector2> pathPointList;
        [SerializeField] private List<MonsterOrderSetting> monsterSpawnOrderSetting;

        public float StartTimeSecond => startTimeSecond;

        public float SpawnIntervalSecond => spawnIntervalSecond;
        public List<Vector2> PathPointList => pathPointList;

        public List<IMonsterSetting> GetMonsterOrderList()
        {
            List<IMonsterSetting> orderList = new List<IMonsterSetting>();
            foreach (MonsterOrderSetting orderSetting in monsterSpawnOrderSetting)
            {
                for (int i = 0; i < orderSetting.SpawnCount; i++)
                {
                    orderList.Add(orderSetting.MonsterSetting);
                }
            }

            return orderList;
        }
    }
}