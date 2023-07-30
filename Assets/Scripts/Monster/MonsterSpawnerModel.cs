using System;

namespace GameCore
{
    public class MonsterSpawnerModel
    {
        public event Action OnSpawnMonster;
        public bool CanSpawnNext => false;
        private IAttackWave AttackWave { get; }

        public MonsterSpawnerModel(IAttackWave attackWave)
        {
            AttackWave = attackWave;
        }

        public void Spawn()
        {
            if (AttackWave.GetCurrentSpawnCount < AttackWave.GetMaxSpawnCount)
                OnSpawnMonster?.Invoke();
        }
    }
}