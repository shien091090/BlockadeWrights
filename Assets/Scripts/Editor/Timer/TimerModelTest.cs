using System;
using System.Linq;
using NSubstitute;
using NUnit.Framework;

namespace GameCore.Tests.Timer
{
    public class TimerModelTest
    {
        private TimerModel timerModel;
        private ITimeManager timeManager;
        private ITimerView timerView;
        private Action onTimeUpEvent;

        [SetUp]
        public void Setup()
        {
            onTimeUpEvent = Substitute.For<Action>();

            timeManager = Substitute.For<ITimeManager>();
            GivenDeltaTime(1);
            timerModel = new TimerModel(timeManager);

            timerView = Substitute.For<ITimerView>();
            timerModel.Bind(timerView);
        }

        [Test]
        //計時器未設置時限
        public void no_count_down_time_setting()
        {
            GivenDeltaTime(1);

            timerModel.StartCountDown(0);
            timerModel.Update();

            ShouldTimerPlaying(false);
            ShouldSetTimerActive(false);
            CurrentTimeShouldBe(0);
        }

        [Test]
        //時限設置為負數
        public void count_down_time_setting_is_negative()
        {
            GivenDeltaTime(1);

            timerModel.StartCountDown(-1);
            timerModel.Update();

            ShouldTimerPlaying(false);
            ShouldSetTimerActive(false);
            CurrentTimeShouldBe(0);
        }

        [Test]
        //刷新倒數時間
        public void update_count_down_time()
        {
            GivenDeltaTime(0.5f);

            timerModel.StartCountDown(10);
            timerModel.Update();

            ShouldTimerPlaying(true);
            ShouldSetTimerActive(true);
            CurrentTimeShouldBe(9.5f);
        }

        [Test]
        //刷新倒數時間結束
        public void update_count_down_time_to_end()
        {
            GivenDeltaTime(3);

            timerModel.StartCountDown(10, onTimeUpEvent);
            timerModel.Update();
            timerModel.Update();
            timerModel.Update();

            ShouldTimerPlaying(true);
            CurrentTimeShouldBe(1);

            timerModel.Update();

            ShouldTimerPlaying(false);
            CurrentTimeShouldBe(0);
            ShouldSetTimerActive(false);
            ShouldTriggerTimeUpEvent();
        }

        [Test]
        //倒數到一半時重新開始倒數
        public void restart_count_down_time()
        {
            GivenDeltaTime(1);

            timerModel.StartCountDown(10);
            timerModel.Update();
            timerModel.Update();
            timerModel.Update();

            ShouldTimerPlaying(true);
            CurrentTimeShouldBe(7);

            timerModel.StartCountDown(15);
            timerModel.Update();
            timerModel.Update();
            timerModel.Update();

            ShouldTimerPlaying(true);
            CurrentTimeShouldBe(12);
        }

        [Test]
        //設置倒數時間, 驗證尚未刷新時的顯示時間文字
        public void update_count_down_time_text_before_update()
        {
            timerModel.StartCountDown(155);

            TimerTextShouldBe("02:35");
        }

        [Test]
        //尚未設置倒數時間, 顯示預設文字&不顯示面板
        public void update_count_down_time_text_and_set_active_before_setting()
        {
            ShouldSetTimerActive(false);
            TimerTextShouldBe("00:00");
        }

        [Test]
        //設置倒數時間, 驗證刷新後顯示時間文字
        public void update_count_down_time_text()
        {
            GivenDeltaTime(1);

            timerModel.StartCountDown(62);
            timerModel.Update();

            ShouldSetTimeText("01:01");

            timerModel.Update();

            ShouldSetTimeText("01:00");

            timerModel.Update();

            ShouldSetTimeText("00:59");
        }

        [Test]
        //設置倒數時間, 驗證刷新間隔為小數點時的顯示時間文字
        public void update_count_down_time_text_with_decimal()
        {
            GivenDeltaTime(0.5f);

            timerModel.StartCountDown(61);
            timerModel.Update();

            ShouldSetTimeText("01:01");

            timerModel.Update();

            ShouldSetTimeText("01:00");

            timerModel.Update();

            ShouldSetTimeText("01:00");

            timerModel.Update();

            ShouldSetTimeText("00:59");
        }

        private void GivenDeltaTime(float deltaTime)
        {
            timeManager.DeltaTime.Returns(deltaTime);
        }

        private void ShouldTriggerTimeUpEvent()
        {
            onTimeUpEvent.Received().Invoke();
        }

        private void ShouldSetTimerActive(bool expectedActive)
        {
            bool argument = (bool)timerView
                .ReceivedCalls()
                .Last(x => x.GetMethodInfo().Name == "SetTimerActive")
                .GetArguments()[0];

            Assert.AreEqual(expectedActive, argument);
        }

        private void ShouldSetTimeText(string expectedTimeText)
        {
            string argument = (string)timerView
                .ReceivedCalls()
                .Last(x => x.GetMethodInfo().Name == "SetTimeText")
                .GetArguments()[0];

            Assert.AreEqual(expectedTimeText, argument);
        }

        private void TimerTextShouldBe(string expectedTimeText)
        {
            Assert.AreEqual(expectedTimeText, timerModel.CurrentTimeText);
        }

        private void CurrentTimeShouldBe(float expectedCurrentTime)
        {
            Assert.AreEqual(expectedCurrentTime, timerModel.CurrentTime);
        }

        private void ShouldTimerPlaying(bool expectedIsPlaying)
        {
            Assert.AreEqual(expectedIsPlaying, timerModel.IsTimerPlaying);
        }
    }
}