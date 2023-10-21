using UnityEngine;

namespace GameCore
{
    public class GameProcessModel : IGameProcessModel
    {
        private readonly IMonsterSpawner monsterSpawner;
        private readonly ITimeManager timeManager;
        private readonly TimerModel timerModel;
        private readonly IPlayerSetting playerSetting;
        private IFortressModel fortressModel;
        private IGameProcessView gameProcessView;
        private int currentRemainMonsterCount;

        public bool IsStartGame { get; private set; }

        public GameProcessModel(IMonsterSpawner monsterSpawner, ITimeManager timeManager, IPlayerSetting playerSetting)
        {
            IsStartGame = false;

            this.monsterSpawner = monsterSpawner;
            this.timeManager = timeManager;
            this.playerSetting = playerSetting;

            timerModel = new TimerModel(timeManager);
        }

        public void Update()
        {
            if (IsStartGame == false)
                return;

            monsterSpawner.CheckUpdateSpawn(timeManager.DeltaTime);
        }

        public void StartGame()
        {
            IsStartGame = true;
            currentRemainMonsterCount = monsterSpawner.TotalMonsterCount;

            RefreshRemainMonsterCountHint();
            CheckStartTimer();
        }

        public void Bind(IGameProcessView gameProcessView)
        {
            this.gameProcessView = gameProcessView;
            IsStartGame = false;

            fortressModel = new FortressModel(playerSetting.FortressHp);

            gameProcessView.GetTimerView.BindModel(timerModel);
            gameProcessView.GetFortressView.BindModel(fortressModel);
            gameProcessView.GetWaveHintView.SetWaveHint(monsterSpawner.GetWaveHint);
            gameProcessView.GetRemainMonsterHintView.SetRemainCountHint("0/0");
            gameProcessView.SetGameOverPanelActive(false);
            gameProcessView.SetQuestCompletePanelActive(false);

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

        private void RefreshRemainMonsterCountHint()
        {
            gameProcessView.GetRemainMonsterHintView.SetRemainCountHint($"{currentRemainMonsterCount}/{monsterSpawner.TotalMonsterCount}");
        }

        private void OnFortressDestroy()
        {
            gameProcessView.SetGameOverPanelActive(true);
        }

        private void OnSpawnMonster(IMonsterModel monsterModel)
        {
            IMonsterView monsterView = gameProcessView.SpawnMonsterView(monsterModel);
            monsterModel.Bind(monsterView);
            monsterModel.SetAttackTarget(fortressModel);

            monsterModel.OnDead -= OnSubtractRemainMonster;
            monsterModel.OnDead += OnSubtractRemainMonster;
            
            monsterModel.OnArrivedGoal -= OnSubtractRemainMonster;
            monsterModel.OnArrivedGoal += OnSubtractRemainMonster;
        }

        private void OnSubtractRemainMonster()
        {
            currentRemainMonsterCount--;
            RefreshRemainMonsterCountHint();

            if (currentRemainMonsterCount <= 0)
                gameProcessView.SetQuestCompletePanelActive(true);
        }

        private void OnStartNextWave()
        {
            gameProcessView.GetWaveHintView.SetWaveHint(monsterSpawner.GetWaveHint);
        }
    }
}