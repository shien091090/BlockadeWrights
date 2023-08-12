using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    [System.Serializable]
    public class AttackWaveSetting
    {
        [SerializeField] private float startTimeSecond;
        [SerializeField] private int maxSpawnCount;
        [SerializeField] private float spawnIntervalSecond;
        [SerializeField] private List<Vector2> pathPointList;

        public float StartTimeSecond => startTimeSecond;
        public int MaxSpawnCount => maxSpawnCount;
        public float SpawnIntervalSecond => spawnIntervalSecond;
        public List<Vector2> PathPointList => pathPointList;
    }
}