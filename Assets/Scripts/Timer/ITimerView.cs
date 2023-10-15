namespace GameCore
{
    public interface ITimerView
    {
        void SetTimeText(string timeText);
        void SetTimerActive(bool isActive);
        void BindModel(TimerModel timerModel);
    }
}