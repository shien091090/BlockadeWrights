using System;
using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace GameCore.Tests.GameProcess
{
    public class GameProcessModelTest
    {
        private IMonsterSpawner monsterSpawner;
        private IGameProcessView gameProcessView;
        private ITimeManager timeManager;
        private TimerModel timerModel;
        private GameProcessModel gameProcessModel;
        private IWaveHintView waveHintView;
        private IPlayerSetting playerSetting;
        private IFortressModel fortressModel;
        private IFortressView fortressView;
        private ITimerView timerView;
        private IRemainMonsterHintView remainMonsterHintView;

        [SetUp]
        public void Setup()
        {
            timeManager = Substitute.For<ITimeManager>();
            GivenDeltaTime(1);

            gameProcessView = Substitute.For<IGameProcessView>();
            timerView = Substitute.For<ITimerView>();
            gameProcessView.GetTimerView.Returns(timerView);

            timerView.When(x => x.BindModel(Arg.Any<TimerModel>())).Do((bindModelFunc) =>
            {
                TimerModel model = bindModelFunc.Arg<TimerModel>();
                timerModel = model;
            });

            fortressView = Substitute.For<IFortressView>();
            gameProcessView.GetFortressView.Returns(fortressView);

            fortressView.When(x => x.BindModel(Arg.Any<IFortressModel>())).Do((bindModelFunc) =>
            {
                IFortressModel model = bindModelFunc.Arg<IFortressModel>();
                fortressModel = model;
            });

            waveHintView = Substitute.For<IWaveHintView>();
            gameProcessView.GetWaveHintView.Returns(waveHintView);

            remainMonsterHintView = Substitute.For<IRemainMonsterHintView>();
            gameProcessView.GetRemainMonsterHintView.Returns(remainMonsterHintView);

            monsterSpawner = Substitute.For<IMonsterSpawner>();
            playerSetting = Substitute.For<IPlayerSetting>();
            gameProcessModel = new GameProcessModel(monsterSpawner, timeManager, playerSetting);
        }

        [Test]
        //初始化綁定流程
        public void init_bind_process()
        {
            GivenFortressHp(5);
            GivenWaveHint("0/5");
            GivenStartTimeSeconds(5);
            GivenIsNeedCountDown(true);

            gameProcessModel.Bind(gameProcessView);

            ShouldTimerViewCallBindModel();
            WaveHintShouldBe("0/5");
            ShouldFortressViewCallBindModel(5);
            ShouldTimerPlaying(false);
            ShouldIsStartGame(false);
            GameOverPanelShouldBeActive(false);
            QuestCompletePanelShouldBeActive(false);
        }

        [Test]
        //遊戲開始後才會開始產怪倒數計時
        public void start_game_then_start_spawn_monster_count_down()
        {
            GivenIsNeedCountDown(true);
            GivenStartTimeSeconds(5);

            gameProcessModel.Bind(gameProcessView);
            gameProcessModel.Update();

            ShouldIsStartGame(false);
            ShouldTimerPlaying(false);
            ShouldNotUpdateToSpawnMonster();

            gameProcessModel.StartGame();
            gameProcessModel.Update();

            ShouldIsStartGame(true);
            ShouldTimerPlaying(true);
            ShouldUpdateToSpawnMonster();
        }

        [Test]
        //第一波產怪需等待時間, 顯示倒數計時
        public void first_wave_spawn_monster_need_wait_time_then_show_count_down()
        {
            GivenIsNeedCountDown(true);
            GivenStartTimeSeconds(10);

            gameProcessModel.Bind(gameProcessView);
            gameProcessModel.StartGame();

            CurrentTimeShouldBe("00:10");
            ShouldTimerPlaying(true);
        }

        [Test]
        //第一波產怪尚未開始時, 波次提示顯示為0
        public void first_wave_spawn_monster_not_start_then_wave_hint_is_0()
        {
            GivenIsNeedCountDown(true);
            GivenStartTimeSeconds(5);
            GivenWaveHint("0/5");

            gameProcessModel.Bind(gameProcessView);

            WaveHintShouldBe("0/5");
        }

        [Test]
        //第一波產怪開始時, 波次從0變1
        public void first_wave_spawn_monster_start_then_wave_change_from_0_to_1()
        {
            GivenIsNeedCountDown(false);
            GivenStartTimeSeconds(0);
            GivenWaveHint("0/5");

            gameProcessModel.Bind(gameProcessView);

            GivenWaveHint("1/5");
            CallStartNextWaveEvent();

            ShouldCallSetWaveHint(2);
            WaveHintShouldBe("1/5");
        }

        [Test]
        //等待第二波產怪, 不顯示倒數計時
        public void wait_second_wave_spawn_monster_then_not_show_count_down()
        {
            GivenIsNeedCountDown(true);
            GivenStartTimeSeconds(1);

            gameProcessModel.Bind(gameProcessView);
            gameProcessModel.StartGame();

            timerModel.Update();
            timerModel.Update();
            timerModel.Update();

            CurrentTimeShouldBe("00:00");
            ShouldTimerPlaying(false);
        }

        [Test]
        //產怪時, 產生怪物View並綁定Model
        public void spawn_monster_then_create_monster_view_and_bind_model()
        {
            gameProcessModel.Bind(gameProcessView);

            IMonsterModel monsterModel = CreateMonsterModel(55);
            CallSpawnMonster(monsterModel);

            ShouldSpawnMonsterView(55);
            ShouldMonsterModelBind(monsterModel);
            ShouldMonsterSetAttackTarget(monsterModel);
        }

        [Test]
        //主堡被破壞, 顯示失敗畫面
        public void fortress_destroyed_then_show_game_over()
        {
            GivenFortressHp(3);

            gameProcessModel.Bind(gameProcessView);

            fortressModel.Damage();
            fortressModel.Damage();
            fortressModel.Damage();

            GameOverPanelShouldBeActive(true);
        }

        [Test]
        //怪物死亡, 刷新剩餘怪物數量提示
        public void monster_dead_then_refresh_monster_count_hint()
        {
            GivenTotalMonsterCount(15);

            gameProcessModel.Bind(gameProcessView);
            gameProcessModel.StartGame();

            RemainMonsterHintShouldBe("15/15");

            IMonsterModel monsterModel1 = CreateMonsterModel(5);
            IMonsterModel monsterModel2 = CreateMonsterModel(5);
            IMonsterModel monsterModel3 = CreateMonsterModel(5);

            CallSpawnMonster(monsterModel1);
            CallSpawnMonster(monsterModel2);
            CallSpawnMonster(monsterModel3);

            CallDeadEvent(monsterModel1);
            CallDeadEvent(monsterModel2);
            CallDeadEvent(monsterModel3);

            RemainMonsterHintShouldBe("12/15");
        }

        [Test]
        //所有怪物死亡, 顯示勝利畫面
        public void all_monster_dead_then_show_quest_complete()
        {
            GivenTotalMonsterCount(3);

            gameProcessModel.Bind(gameProcessView);
            gameProcessModel.StartGame();

            IMonsterModel monsterModel1 = CreateMonsterModel(5);
            IMonsterModel monsterModel2 = CreateMonsterModel(5);
            IMonsterModel monsterModel3 = CreateMonsterModel(5);

            CallSpawnMonster(monsterModel1);
            CallSpawnMonster(monsterModel2);
            CallSpawnMonster(monsterModel3);

            CallDeadEvent(monsterModel1);
            CallDeadEvent(monsterModel2);
            CallDeadEvent(monsterModel3);

            RemainMonsterHintShouldBe("0/3");
            QuestCompletePanelShouldBeActive(true);
        }

        [Test]
        //部分怪物攻擊主堡, 主堡尚未破壞, 其餘怪物死亡, 顯示勝利畫面
        public void some_monster_attack_fortress_and_other_monster_dead_then_show_quest_complete()
        {
            GivenTotalMonsterCount(3);

            gameProcessModel.Bind(gameProcessView);
            gameProcessModel.StartGame();

            RemainMonsterHintShouldBe("3/3");
            
            IMonsterModel monsterModel1 = CreateMonsterModel(5);
            IMonsterModel monsterModel2 = CreateMonsterModel(5);
            IMonsterModel monsterModel3 = CreateMonsterModel(5);

            CallSpawnMonster(monsterModel1);
            CallSpawnMonster(monsterModel2);
            CallSpawnMonster(monsterModel3);

            CallDeadEvent(monsterModel1);
            CallDeadEvent(monsterModel2);
            CallArrivedGoalEvent(monsterModel3);

            RemainMonsterHintShouldBe("0/3");
            QuestCompletePanelShouldBeActive(true);
        }

        private void GivenTotalMonsterCount(int expectedTotalCount)
        {
            monsterSpawner.TotalMonsterCount.Returns(expectedTotalCount);
        }

        private void GivenFortressHp(int hp)
        {
            playerSetting.FortressHp.Returns(hp);
        }

        private void GivenWaveHint(string waveHint)
        {
            monsterSpawner.GetWaveHint.Returns(waveHint);
        }

        private void GivenStartTimeSeconds(int startTimeSeconds)
        {
            monsterSpawner.GetStartTimeSeconds().Returns(startTimeSeconds);
        }

        private void GivenIsNeedCountDown(bool isNeed)
        {
            monsterSpawner.IsNeedCountDownToSpawnMonster().Returns(isNeed);
        }

        private void GivenDeltaTime(int deltaTime)
        {
            timeManager.DeltaTime.Returns(deltaTime);
        }

        private void CallArrivedGoalEvent(IMonsterModel monsterModel)
        {
            monsterModel.OnArrivedGoal += Raise.Event<Action>();
        }

        private void CallDeadEvent(IMonsterModel monsterModel)
        {
            monsterModel.OnDead += Raise.Event<Action>();
        }

        private void CallSpawnMonster(IMonsterModel monsterModel)
        {
            monsterSpawner.OnSpawnMonster += Raise.Event<Action<IMonsterModel>>(monsterModel);
        }

        private void CallStartNextWaveEvent()
        {
            monsterSpawner.OnStartNextWave += Raise.Event<Action>();
        }

        private void RemainMonsterHintShouldBe(string expectedHint)
        {
            string argument = (string)remainMonsterHintView.ReceivedCalls().Last(x => x.GetMethodInfo().Name == "SetRemainCountHint").GetArguments()[0];
            Assert.AreEqual(expectedHint, argument);
        }

        private void QuestCompletePanelShouldBeActive(bool expectedActive)
        {
            bool argument = (bool)gameProcessView.ReceivedCalls().Last(x => x.GetMethodInfo().Name == "SetQuestCompletePanelActive").GetArguments()[0];
            Assert.AreEqual(expectedActive, argument);
        }

        private void GameOverPanelShouldBeActive(bool expectedActive)
        {
            bool argument = (bool)gameProcessView.ReceivedCalls().Last(x => x.GetMethodInfo().Name == "SetGameOverPanelActive").GetArguments()[0];
            Assert.AreEqual(expectedActive, argument);
        }

        private void ShouldUpdateToSpawnMonster()
        {
            monsterSpawner.Received().CheckUpdateSpawn(Arg.Any<float>());
        }

        private void ShouldNotUpdateToSpawnMonster()
        {
            monsterSpawner.DidNotReceive().CheckUpdateSpawn(Arg.Any<float>());
        }

        private void ShouldIsStartGame(bool expectedStartGame)
        {
            Assert.AreEqual(expectedStartGame, gameProcessModel.IsStartGame);
        }

        private void ShouldFortressViewCallBindModel(float expectedFortressHp)
        {
            fortressView.Received().BindModel(Arg.Is<IFortressModel>(fortressModel => fortressModel.CurrentHp == expectedFortressHp));
        }

        private void ShouldTimerViewCallBindModel()
        {
            timerView.Received().BindModel(Arg.Any<TimerModel>());
        }

        private void ShouldDestroyHintActive(bool expectedActive)
        {
            bool argument = (bool)fortressView
                .ReceivedCalls()
                .Last(x => x.GetMethodInfo().Name == "SetDestroyHintActive")
                .GetArguments()[0];

            Assert.AreEqual(expectedActive, argument);
        }

        private void ShouldMonsterSetAttackTarget(IMonsterModel monsterModel)
        {
            monsterModel.Received().SetAttackTarget(Arg.Any<IFortressModel>());
        }

        private void ShouldMonsterModelBind(IMonsterModel monsterModel)
        {
            monsterModel.Received().Bind(Arg.Any<IMonsterView>());
        }

        private void ShouldSpawnMonsterView(float expectedMonsterHp)
        {
            IMonsterModel argument = (IMonsterModel)gameProcessView.ReceivedCalls().Last(x => x.GetMethodInfo().Name == "SpawnMonsterView").GetArguments()[0];
            Assert.AreEqual(expectedMonsterHp, argument.GetHp);
        }

        private void ShouldTimerPlaying(bool expectedIsPlaying)
        {
            Assert.AreEqual(expectedIsPlaying, timerModel.IsTimerPlaying);
        }

        private void ShouldCallSetWaveHint(int callTimes)
        {
            waveHintView.Received(callTimes).SetWaveHint(Arg.Any<string>());
        }

        private void WaveHintShouldBe(string expectedWaveHint)
        {
            string argument = (string)waveHintView.ReceivedCalls().Last(x => x.GetMethodInfo().Name == "SetWaveHint").GetArguments()[0];
            Assert.AreEqual(expectedWaveHint, argument);
        }

        private void CurrentTimeShouldBe(string expectedTimeText)
        {
            Assert.AreEqual(expectedTimeText, timerModel.CurrentTimeText);
        }

        private IMonsterModel CreateMonsterModel(float hp)
        {
            IMonsterModel monsterModel = Substitute.For<IMonsterModel>();
            monsterModel.GetHp.Returns(hp);
            return monsterModel;
        }
    }
}