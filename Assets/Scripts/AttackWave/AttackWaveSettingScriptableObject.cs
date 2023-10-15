using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore
{
    public class AttackWaveSettingScriptableObject : SerializedScriptableObject, IAttackWaveSetting
    {
        [SerializeField] private AttackWaveSetting[] attackWaveSettings;

        public AttackWave[] GetAttackWaves()
        {
            AttackWave[] attackWaves = new AttackWave[attackWaveSettings.Length];
            AttackWaveSetting[] orderAttackWaveSettings = attackWaveSettings.OrderBy(x => x.StartTimeSecond).ToArray();

            for (int i = 0; i < attackWaves.Length; i++)
            {
                AttackWaveSetting attackWaveSetting = orderAttackWaveSettings[i];
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