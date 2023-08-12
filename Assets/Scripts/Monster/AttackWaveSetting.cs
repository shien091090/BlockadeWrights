using UnityEngine;

namespace GameCore
{
    [System.Serializable]
    public class AttackWaveSetting
    {
        [SerializeField] private float startTimeSecond;
        [SerializeField] private int maxSpawnCount;
        [SerializeField] private float spawnIntervalSecond;

        public float StartTimeSecond => startTimeSecond;
        public int MaxSpawnCount => maxSpawnCount;
        public float SpawnIntervalSecond => spawnIntervalSecond;
    }
}