using NUnit.Framework;

namespace GameCore.Tests.Timer
{
    public class TimerModelTest
    {
        [Test]
        //計時器未設置時限
        public void no_count_down_time_setting()
        {
            TimerModel timerModel = new TimerModel();
            timerModel.StartCountDown(0);
            timerModel.UpdateCountDownTime(1);
            
            Assert.AreEqual(false, timerModel.IsTimerPlaying);
            Assert.AreEqual(0, timerModel.CurrentTime);
        }

        [Test]
        //時限設置為負數
        public void count_down_time_setting_is_negative()
        {
            TimerModel timerModel = new TimerModel();
            timerModel.StartCountDown(-1);
            timerModel.UpdateCountDownTime(1);
            
            Assert.AreEqual(false, timerModel.IsTimerPlaying);
            Assert.AreEqual(0, timerModel.CurrentTime);
        }
        
        //刷新倒數時間
        //刷新倒數時間結束
    }
}