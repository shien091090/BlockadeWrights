using UnityEngine;

namespace GameCore
{
    public class AttackWaveSettingScriptableObject : ScriptableObject
    {
        [SerializeField] private AttackWaveSetting[] attackWaveSettings;
        [SerializeField] private float monsterHp;

        public float MonsterHp => monsterHp;

        public AttackWave[] GetAttackWaves()
        {
            AttackWave[] attackWaves = new AttackWave[attackWaveSettings.Length];
            for (int i = 0; i < attackWaves.Length; i++)
            {
                attackWaves[i] = new AttackWave(
                    attackWaveSettings[i].MaxSpawnCount,
                    attackWaveSettings[i].SpawnIntervalSecond,
                    attackWaveSettings[i].StartTimeSecond,
                    attackWaveSettings[i].PathPointList);
            }

            return attackWaves;
        }

    }
}