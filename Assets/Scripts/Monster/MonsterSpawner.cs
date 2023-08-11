using System;

namespace GameCore
{
    public class MonsterSpawner
    {
        private float currentTimer;
        private AttackWave[] attackWaves;
        private int currentWaveIndex;

        public event Action OnSpawnMonster;

        public bool CanSpawnNext => attackWaves[currentWaveIndex].GetCurrentSpawnCount < attackWaves[currentWaveIndex].GetMaxSpawnCount;

        public void CheckUpdateSpawn(float deltaTime)
        {
            if (CanSpawnNext == false)
                return;

            currentTimer += deltaTime;
            if (currentTimer < attackWaves[currentWaveIndex].SpawnInterval)
                return;

            currentTimer = 0;
            attackWaves[currentWaveIndex].AddSpawnCount(1);
            OnSpawnMonster?.Invoke();
        }

        public void SetAttackWave(params AttackWave[] attackWaves)
        {
            this.attackWaves = attackWaves;
        }
    }
}