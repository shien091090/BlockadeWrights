using UnityEngine;

namespace GameCore
{
    public class GameProcessModel
    {
        private readonly IMonsterSpawner monsterSpawner;
        private readonly TimerModel timerModel;
        private readonly IPlayerSetting playerSetting;
        private IFortressModel fortressModel;
        private IGameProcessView gameProcessView;

        public GameProcessModel(IMonsterSpawner monsterSpawner, ITimeManager timeManager, IPlayerSetting playerSetting)
        {
            this.monsterSpawner = monsterSpawner;
            this.playerSetting = playerSetting;
            timerModel = new TimerModel(timeManager);
        }

        public void Init()
        {
            CheckStartTimer();
        }

        public void Bind(IGameProcessView gameProcessView)
        {
            this.gameProcessView = gameProcessView;
            fortressModel = new FortressModel(playerSetting.FortressHp);

            gameProcessView.GetTimerView.BindModel(timerModel);
            gameProcessView.GetWaveHintView.SetWaveHint(monsterSpawner.GetWaveHint);
            gameProcessView.GetFortressView.BindModel(fortressModel);

            SetEventRegister();
        }

        private void SetEventRegister()
        {
            monsterSpawner.OnSpawnMonster -= OnSpawnMonster;
            monsterSpawner.OnSpawnMonster += OnSpawnMonster;

            monsterSpawner.OnStartNextWave -= OnStartNextWave;
            monsterSpawner.OnStartNextWave += OnStartNextWave;

            fortressModel.OnFortressDestroy -= OnFortressDestroy;
            fortressModel.OnFortressDestroy += OnFortressDestroy;
        }

        private void CheckStartTimer()
        {
            if (monsterSpawner.IsNeedCountDownToSpawnMonster())
                timerModel.StartCountDown(monsterSpawner.GetStartTimeSeconds());
        }

        private void OnFortressDestroy()
        {
            gameProcessView.GetFortressView.SetDestroyHintActive(true);
        }

        private void OnSpawnMonster(IMonsterModel monsterModel)
        {
            IMonsterView monsterView = gameProcessView.SpawnMonsterView(monsterModel);
            monsterModel.Bind(monsterView);
            monsterModel.SetAttackTarget(fortressModel);
        }

        private void OnStartNextWave()
        {
            gameProcessView.GetWaveHintView.SetWaveHint(monsterSpawner.GetWaveHint);
        }
    }
}