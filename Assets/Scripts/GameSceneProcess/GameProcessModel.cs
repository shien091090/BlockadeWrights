namespace GameCore
{
    public class GameProcessModel
    {
        private readonly IMonsterSpawner monsterSpawner;
        private readonly TimerModel timerModel;
        private IGameProcessView gameProcessView;

        public GameProcessModel(IMonsterSpawner monsterSpawner, ITimeManager timeManager)
        {
            this.monsterSpawner = monsterSpawner;
            timerModel = new TimerModel(timeManager);
        }

        public void Bind(IGameProcessView gameProcessView)
        {
            this.gameProcessView = gameProcessView;
            gameProcessView.GetTimerView.BindModel(timerModel);
            CheckStartTimer();
        }

        private void CheckStartTimer()
        {
            if (monsterSpawner.IsNeedCountDownToSpawnMonster())
                timerModel.StartCountDown(monsterSpawner.GetStartTimeSeconds());
        }
    }
}