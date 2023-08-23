using System;
using NSubstitute;
using NUnit.Framework;

namespace GameCore.Tests.Timer
{
    public class TimerModelTest
    {
        private TimerModel timerModel;
        private Action onTimeUpCallback;
        private Action<string> onUpdateTimeText;

        [SetUp]
        public void Setup()
        {
            timerModel = new TimerModel();

            onTimeUpCallback = Substitute.For<Action>();

            onUpdateTimeText = Substitute.For<Action<string>>();
            timerModel.onUpdateTimeText += onUpdateTimeText;
        }

        [Test]
        //計時器未設置時限
        public void no_count_down_time_setting()
        {
            timerModel.StartCountDown(0, onTimeUpCallback);
            timerModel.UpdateCountDownTime(1);

            ShouldTimerPlaying(false);
            CurrentTimeShouldBe(0);
        }

        [Test]
        //時限設置為負數
        public void count_down_time_setting_is_negative()
        {
            timerModel.StartCountDown(-1, onTimeUpCallback);
            timerModel.UpdateCountDownTime(1);

            ShouldTimerPlaying(false);
            CurrentTimeShouldBe(0);
        }

        [Test]
        //刷新倒數時間
        public void update_count_down_time()
        {
            timerModel.StartCountDown(10, onTimeUpCallback);
            timerModel.UpdateCountDownTime(0.5f);

            ShouldTimerPlaying(true);
            CurrentTimeShouldBe(9.5f);
        }

        [Test]
        //刷新倒數時間結束
        public void update_count_down_time_to_end()
        {
            timerModel.StartCountDown(10, onTimeUpCallback);
            timerModel.UpdateCountDownTime(3);
            timerModel.UpdateCountDownTime(3);
            timerModel.UpdateCountDownTime(3);

            ShouldTimerPlaying(true);
            CurrentTimeShouldBe(1);

            timerModel.UpdateCountDownTime(3);

            ShouldTimerPlaying(false);
            CurrentTimeShouldBe(0);
            ShouldTriggerTimeUpEvent();
        }

        [Test]
        //倒數到一半時重新開始倒數
        public void restart_count_down_time()
        {
            timerModel.StartCountDown(10, onTimeUpCallback);
            timerModel.UpdateCountDownTime(1);
            timerModel.UpdateCountDownTime(1);
            timerModel.UpdateCountDownTime(1);

            ShouldTimerPlaying(true);
            CurrentTimeShouldBe(7);

            timerModel.StartCountDown(15, onTimeUpCallback);
            timerModel.UpdateCountDownTime(1);
            timerModel.UpdateCountDownTime(1);
            timerModel.UpdateCountDownTime(1);

            ShouldTimerPlaying(true);
            CurrentTimeShouldBe(12);
        }

        [Test]
        //設置倒數時間, 驗證尚未刷新時的顯示時間文字
        public void update_count_down_time_text_before_update()
        {
            timerModel.StartCountDown(155, onTimeUpCallback);

            TimerTextShouldBe("02:35");
        }

        [Test]
        //設置倒數時間, 驗證刷新後顯示時間文字
        public void update_count_down_time_text()
        {
            timerModel.StartCountDown(62, onTimeUpCallback);
            timerModel.UpdateCountDownTime(1);

            ShouldReceiveUpdateTimeTextEvent("01:01");

            timerModel.UpdateCountDownTime(1);

            ShouldReceiveUpdateTimeTextEvent("01:00");

            timerModel.UpdateCountDownTime(1);

            ShouldReceiveUpdateTimeTextEvent("00:59");
        }

        [Test]
        //設置倒數時間, 驗證刷新間隔為小數點時的顯示時間文字
        public void update_count_down_time_text_with_decimal()
        {
            timerModel.StartCountDown(61, onTimeUpCallback);
            timerModel.UpdateCountDownTime(0.5f);

            ShouldReceiveUpdateTimeTextEvent("01:01");

            timerModel.UpdateCountDownTime(0.5f);

            ShouldReceiveUpdateTimeTextEvent("01:00");

            timerModel.UpdateCountDownTime(0.5f);

            ShouldReceiveUpdateTimeTextEvent("01:00");

            timerModel.UpdateCountDownTime(0.5f);

            ShouldReceiveUpdateTimeTextEvent("00:59");
        }

        private void ShouldReceiveUpdateTimeTextEvent(string expectedTimeText)
        {
            onUpdateTimeText.Received(1).Invoke(expectedTimeText);
        }

        private void TimerTextShouldBe(string expectedTimeText)
        {
            Assert.AreEqual(expectedTimeText, timerModel.CurrentTimeText);
        }

        private void ShouldTriggerTimeUpEvent()
        {
            onTimeUpCallback.Received(1).Invoke();
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