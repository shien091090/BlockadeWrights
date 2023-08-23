namespace GameCore
{
    public class TimerModel
    {
        public bool IsTimerPlaying { get; private set; }
        public float CurrentTime { get; private set; }

        public void StartCountDown(float countDownTime)
        {
            if (countDownTime <= 0)
            {
                CurrentTime = 0;
                IsTimerPlaying = false;
                return;
            }

            CurrentTime = countDownTime;
            IsTimerPlaying = true;
        }

        public void UpdateCountDownTime(float deltaTime)
        {
            if (IsTimerPlaying == false)
                return;

            CurrentTime -= deltaTime;

            if (CurrentTime <= 0)
            {
                CurrentTime = 0;
                IsTimerPlaying = false;
            }
        }
    }
}