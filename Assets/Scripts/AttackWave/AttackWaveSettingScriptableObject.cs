using UnityEngine;

namespace GameCore
{
    public class AttackWaveSettingScriptableObject : ScriptableObject, IAttackWaveSetting
    {
        [SerializeField] private AttackWaveSetting[] attackWaveSettings;

        public AttackWave[] GetAttackWaves()
        {
            AttackWave[] attackWaves = new AttackWave[attackWaveSettings.Length];
            for (int i = 0; i < attackWaves.Length; i++)
            {
                AttackWaveSetting attackWaveSetting = attackWaveSettings[i];
                attackWaves[i] = new AttackWave(
                    attackWaveSetting.SpawnIntervalSecond,
                    attackWaveSetting.GetMonsterOrderList(),
                    attackWaveSetting.StartTimeSecond,
                    attackWaveSetting.PathPointList);
            }

            return attackWaves;
        }

    }
}