using System;
using System.Linq;

namespace GameCore
{
    public class MonsterSpawner
    {
        private float currentTimer;
        private AttackWave[] attackWaves;

        public event Action OnSpawnMonster;

        public int GetCurrentWaveIndex { get; private set; }
        public bool HaveNextWave => GetCurrentWaveIndex < attackWaves.Length - 1;

        public bool IsAllWaveSpawnFinished
        {
            get
            {
                if (attackWaves == null)
                    return true;

                return attackWaves.FirstOrDefault(x => x.CanSpawnNext) == null;
            }
        }

        public string GetWaveHint => attackWaves == null ?
            string.Empty :
            $"{GetCurrentWaveIndex + 1}/{attackWaves.Length}";

        private bool IsStartNextWave => currentTimer >= attackWaves[GetCurrentWaveIndex + 1].StartTimeSecond;

        public void CheckUpdateSpawn(float deltaTime)
        {
            if (IsAllWaveSpawnFinished)
                return;

            currentTimer += deltaTime;

            if (HaveNextWave && IsStartNextWave)
                GetCurrentWaveIndex++;

            for (int waveIndex = 0; waveIndex < attackWaves.Length; waveIndex++)
            {
                if (GetCurrentWaveIndex < waveIndex)
                    continue;

                AttackWave attackWave = attackWaves[waveIndex];
                if (attackWave.CanSpawnNext == false)
                    continue;

                if (attackWave.UpdateTimerAndCheckSpawn(deltaTime) == false)
                    continue;

                attackWave.AddSpawnCount(1);
                OnSpawnMonster?.Invoke();
            }
        }

        public void Init(params AttackWave[] attackWaves)
        {
            this.attackWaves = attackWaves;
            GetCurrentWaveIndex = -1;
        }
    }
}