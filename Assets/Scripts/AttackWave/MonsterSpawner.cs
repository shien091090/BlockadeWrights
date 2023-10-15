using System;
using System.Linq;
using Zenject;

namespace GameCore
{
    public class MonsterSpawner : IMonsterSpawner
    {
        [Inject] private ITimeManager timeManager;

        public string GetWaveHint => attackWaves == null ?
            string.Empty :
            $"{GetCurrentWaveIndex + 1}/{attackWaves.Length}";

        private float currentTimer;
        private AttackWave[] attackWaves;
        private readonly IAttackWaveSetting attackWaveSetting;

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

        private bool IsStartNextWave => currentTimer >= attackWaves[GetCurrentWaveIndex + 1].StartTimeSecond;

        public MonsterSpawner(IAttackWaveSetting attackWaveSetting)
        {
            this.attackWaveSetting = attackWaveSetting;
            Init(attackWaveSetting.GetAttackWaves());
        }

        public event Action OnStartNextWave;
        public event Action<IMonsterModel> OnSpawnMonster;

        public void CheckUpdateSpawn(float deltaTime)
        {
            if (IsAllWaveSpawnFinished)
                return;

            currentTimer += deltaTime;

            if (HaveNextWave && IsStartNextWave)
            {
                GetCurrentWaveIndex++;
                OnStartNextWave?.Invoke();
            }

            for (int waveIndex = 0; waveIndex < attackWaves.Length; waveIndex++)
            {
                if (GetCurrentWaveIndex < waveIndex)
                    continue;

                AttackWave attackWave = attackWaves[waveIndex];
                if (attackWave.CanSpawnNext == false)
                    continue;

                if (attackWave.GetCurrentSpawnCount > 0 && attackWave.UpdateTimerAndCheckSpawn(deltaTime) == false)
                    continue;

                IMonsterSetting monsterSetting = attackWave.GetCurrentSpawnMonsterSetting;
                attackWave.AddSpawnCount(1);
                IMonsterModel monsterModel = new MonsterModel(attackWave.GetAttackPath, monsterSetting, timeManager);
                OnSpawnMonster?.Invoke(monsterModel);
            }
        }

        public bool IsNeedCountDownToSpawnMonster()
        {
            if (attackWaveSetting.GetAttackWaves().Length <= 0)
                return false;

            AttackWave firstAttackWave = attackWaveSetting.GetAttackWaves()[0];
            if (firstAttackWave.StartTimeSecond <= 0)
                return false;

            return true;
        }

        public float GetStartTimeSeconds()
        {
            float countDownSeconds = attackWaveSetting.GetAttackWaves()[0].StartTimeSecond;
            return countDownSeconds;
        }

        private void Init(params AttackWave[] attackWaves)
        {
            this.attackWaves = attackWaves;
            GetCurrentWaveIndex = -1;
        }
    }
}