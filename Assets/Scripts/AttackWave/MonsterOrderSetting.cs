using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore
{
    [System.Serializable]
    public class MonsterOrderSetting
    {
        [PreviewField] [ShowInInspector] [VerticalGroup("MonsterLook", 0)] [TableColumnWidth(1)]
        public Sprite GetSprite =>
            monsterSetting == null ?
                null :
                monsterSetting.GetSprite;

        [SerializeField] [VerticalGroup("Setting", 1)] private MonsterScriptableObject monsterSetting;
        [SerializeField] [VerticalGroup("Setting", 1)] private int spawnCount;

        public int SpawnCount => spawnCount;
        public IMonsterSetting MonsterSetting => monsterSetting;
    }
}