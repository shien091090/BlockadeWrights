using System;

namespace GameCore
{
    public class TimerModel
    {
        private readonly ITimeManager timeAdapter;
        private ITimerView timerView;
        
        private Action onTimeUp;

        public bool IsTimerPlaying { get; private set; }
        public float CurrentTime { get; private set; }
        public string CurrentTimeText => ConvertTimeText(CurrentTime);

        public TimerModel(ITimeManager timeAdapter)
        {
            this.timeAdapter = timeAdapter;
        }

        public void Update()
        {
            if (IsTimerPlaying)
                UpdateCountDownTime(timeAdapter.DeltaTime);
        }

        public void StartCountDown(float countDownTime, Action onCountDownFinished = null)
        {
            if (countDownTime <= 0)
            {
                CurrentTime = 0;
                IsTimerPlaying = false;
                return;
            }

            CurrentTime = countDownTime;
            IsTimerPlaying = true;
            timerView?.SetTimerActive(true);
            onTimeUp = onCountDownFinished;
        }

        public void Bind(ITimerView timerView)
        {
            this.timerView = timerView;
            timerView.SetTimerActive(false);
        }

        private string ConvertTimeText(float currentTime)
        {
            int ceilingTime = (int)Math.Ceiling(currentTime);
            int minute = ceilingTime / 60;
            int second = ceilingTime % 60;
            return $"{minute:00}:{second:00}";
        }

        private void UpdateCountDownTime(float deltaTime)
        {
            if (IsTimerPlaying == false)
                return;

            CurrentTime -= deltaTime;

            string timeText = ConvertTimeText(CurrentTime);
            timerView?.SetTimeText(timeText);

            if (CurrentTime > 0)
                return;

            CurrentTime = 0;
            IsTimerPlaying = false;
            timerView?.SetTimerActive(false);
            onTimeUp?.Invoke();
        }
    }
}