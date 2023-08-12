using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class AttackWaveSettingScriptableObject : ScriptableObject
    {
        [SerializeField] private List<Vector2> pathPointList;
        [SerializeField] private AttackWaveSetting[] attackWaveSettings;
        [SerializeField] private float monsterHp;

        public Vector2 StartPoint => pathPointList[0];
        public float MonsterHp => monsterHp;

        public AttackWave[] GetAttackWaves()
        {
            AttackWave[] attackWaves = new AttackWave[attackWaveSettings.Length];
            for (int i = 0; i < attackWaves.Length; i++)
            {
                attackWaves[i] = new AttackWave(
                    attackWaveSettings[i].MaxSpawnCount,
                    attackWaveSettings[i].SpawnIntervalSecond,
                    attackWaveSettings[i].StartTimeSecond);
            }

            return attackWaves;
        }

        public MonsterMovementPath GetPathInfo()
        {
            MonsterMovementPath pathInfo = new MonsterMovementPath();
            foreach (Vector2 pos in pathPointList)
            {
                pathInfo.AddPoint(pos);
            }

            return pathInfo;
        }
    }
}