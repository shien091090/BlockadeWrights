using System;

namespace GameCore
{
    public class FortressModel
    {
        private readonly IMonsterSpawner monsterSpawner;

        public event Action OnFortressDestroy;
        public HealthPointModel HpModel { get; }

        public bool IsInValid => HpModel.IsInValid;
        public float CurrentHp => HpModel.CurrentHp;

        public FortressModel(float mapHp, IMonsterSpawner monsterSpawner)
        {
            HpModel = new HealthPointModel(mapHp);
            this.monsterSpawner = monsterSpawner;
            RegisterEvent();
        }

        private void RegisterEvent()
        {
            monsterSpawner.OnSpawnMonster -= OnSpawnMonster;
            monsterSpawner.OnSpawnMonster += OnSpawnMonster;
        }

        private void Damage()
        {
            HpModel.Damage(1);

            if (HpModel.IsDead)
                OnFortressDestroy?.Invoke();
        }

        private void OnSpawnMonster(IMonsterModel monsterModel)
        {
            monsterModel.OnDamageFort -= Damage;
            monsterModel.OnDamageFort += Damage;
        }
    }
}