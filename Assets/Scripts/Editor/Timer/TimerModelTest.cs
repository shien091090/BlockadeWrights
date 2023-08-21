using NUnit.Framework;

namespace GameCore.Tests.Timer
{
    public class TimerModelTest
    {
        private TimerModel timerModel;

        [SetUp]
        public void Setup()
        {
            timerModel = new TimerModel();
        }

        [Test]
        //計時器未設置時限
        public void no_count_down_time_setting()
        {
            timerModel.StartCountDown(0);
            timerModel.UpdateCountDownTime(1);

            ShouldTimerPlaying(false);
            CurrentTimeShouldBe(0);
        }

        [Test]
        //時限設置為負數
        public void count_down_time_setting_is_negative()
        {
            timerModel.StartCountDown(-1);
            timerModel.UpdateCountDownTime(1);

            ShouldTimerPlaying(false);
            CurrentTimeShouldBe(0);
        }

        [Test]
        //刷新倒數時間
        public void update_count_down_time()
        {
            timerModel.StartCountDown(10);
            timerModel.UpdateCountDownTime(0.5f);

            ShouldTimerPlaying(true);
            CurrentTimeShouldBe(9.5f);
        }

        private void CurrentTimeShouldBe(float expectedCurrentTime)
        {
            Assert.AreEqual(expectedCurrentTime, timerModel.CurrentTime);
        }

        private void ShouldTimerPlaying(bool expectedIsPlaying)
        {
            Assert.AreEqual(expectedIsPlaying, timerModel.IsTimerPlaying);
        }

        //刷新倒數時間結束
    }
}