using System;

namespace GameCore
{
    public class MonsterSpawner
    {
        private AttackWave attackWave;
        private float currentTimer;

        public event Action OnSpawnMonster;

        public bool CanSpawnNext => attackWave.GetCurrentSpawnCount < attackWave.GetMaxSpawnCount;

        public void CheckUpdateSpawn(float deltaTime)
        {
            if (CanSpawnNext == false)
                return;

            currentTimer += deltaTime;
            if (currentTimer < attackWave.SpawnInterval)
                return;

            currentTimer = 0;
            attackWave.AddSpawnCount(1);
            OnSpawnMonster?.Invoke();
        }

        public void SetAttackWave(int spawnCountPerWave, int spawnInterval)
        {
            attackWave = new AttackWave(spawnCountPerWave, spawnInterval);
        }
    }
}