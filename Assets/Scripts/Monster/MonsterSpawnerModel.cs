using System;

namespace GameCore
{
    public class MonsterSpawnerModel
    {
        private AttackWave attackWave;

        public event Action OnSpawnMonster;

        public bool CanSpawnNext => attackWave.GetCurrentSpawnCount < attackWave.GetMaxSpawnCount;

        public void Spawn()
        {
            if (CanSpawnNext == false)
                return;

            attackWave.AddSpawnCount(1);
            OnSpawnMonster?.Invoke();
        }

        public void SetAttackWave(int spawnCountPerWave)
        {
            attackWave = new AttackWave(spawnCountPerWave);
        }
    }
}