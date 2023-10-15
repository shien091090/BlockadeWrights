namespace GameCore
{
    public class GameProcessModel
    {
        private readonly IMonsterSpawner monsterSpawner;
        private readonly TimerModel timerModel;
        private IGameProcessView gameProcessView;
        private IFortressModel fortressModel;

        public GameProcessModel(IMonsterSpawner monsterSpawner, ITimeManager timeManager)
        {
            this.monsterSpawner = monsterSpawner;
            timerModel = new TimerModel(timeManager);
        }

        public void Bind(IGameProcessView gameProcessView)
        {
            this.gameProcessView = gameProcessView;
            gameProcessView.GetTimerView.BindModel(timerModel);
            gameProcessView.GetWaveHintView.SetWaveHint(monsterSpawner.GetWaveHint);
            CheckStartTimer();
            SetEventRegister();
        }

        private void SetEventRegister()
        {
            monsterSpawner.OnSpawnMonster -= OnSpawnMonster;
            monsterSpawner.OnSpawnMonster += OnSpawnMonster;

            monsterSpawner.OnStartNextWave -= OnStartNextWave;
            monsterSpawner.OnStartNextWave += OnStartNextWave;

            // fortressModel.OnFortressDestroy -= OnFortressDestroy;
            // fortressModel.OnFortressDestroy += OnFortressDestroy;
        }

        private void CheckStartTimer()
        {
            if (monsterSpawner.IsNeedCountDownToSpawnMonster())
                timerModel.StartCountDown(monsterSpawner.GetStartTimeSeconds());
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