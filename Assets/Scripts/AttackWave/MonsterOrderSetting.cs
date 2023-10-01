using UnityEngine;

namespace GameCore
{
    [System.Serializable]
    public class MonsterOrderSetting
    {
        [SerializeField] private int spawnCount;
        [SerializeField] private MonsterScriptableObject monsterSetting;

        public int SpawnCount => spawnCount;
        public IMonsterSetting MonsterSetting => monsterSetting;
    }
}