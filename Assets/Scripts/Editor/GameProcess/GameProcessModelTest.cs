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

        [SetUp]
        public void Setup()
        {
            timeManager = Substitute.For<ITimeManager>();
            GivenDeltaTime(1);

            gameProcessView = Substitute.For<IGameProcessView>();
            ITimerView timerView = Substitute.For<ITimerView>();
            gameProcessView.GetTimerView.Returns(timerView);

            timerView.When(x => x.BindModel(Arg.Any<TimerModel>())).Do((bindModelFunc) =>
            {
                TimerModel model = bindModelFunc.Arg<TimerModel>();
                timerModel = model;
            });

            monsterSpawner = Substitute.For<IMonsterSpawner>();
            gameProcessModel = new GameProcessModel(monsterSpawner, timeManager);
        }

        [Test]
        //第一波產怪需等待時間, 顯示倒數計時
        public void first_wave_spawn_monster_need_wait_time_then_show_count_down()
        {
            GivenIsNeedCountDown(true);
            GivenStartTimeSeconds(10);

            gameProcessModel.Bind(gameProcessView);

            CurrentTimeShouldBe("00:10");
        }
        
        [Test]
        //第一波產怪開始時, 波次從0變1
        public void first_wave_spawn_monster_start_then_wave_change_from_0_to_1()
        {
            GivenIsNeedCountDown(true);
            GivenStartTimeSeconds(10);

            gameProcessModel.Bind(gameProcessView);
            
            
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

        private void CurrentTimeShouldBe(string expectedTimeText)
        {
            Assert.AreEqual(expectedTimeText, timerModel.CurrentTimeText);
        }

        //等待第二波產怪, 不顯示倒數計時
        //第二波產怪開始時, 波次從1變2
        //所有怪物死亡, 顯示勝利畫面
        //主堡被破壞, 顯示失敗畫面
    }
}