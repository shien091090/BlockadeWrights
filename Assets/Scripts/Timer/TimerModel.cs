using System;

namespace GameCore
{
    public class TimerModel
    {
        public event Action<string> onUpdateTimeText;
        
        private Action onTimeUpCallback;
        public bool IsTimerPlaying { get; private set; }
        public float CurrentTime { get; private set; }

        public void StartCountDown(float countDownTime, Action callback)
        {
            if (countDownTime <= 0)
            {
                CurrentTime = 0;
                onTimeUpCallback = null;
                IsTimerPlaying = false;
                return;
            }

            CurrentTime = countDownTime;
            onTimeUpCallback = callback;
            IsTimerPlaying = true;
        }

        public void UpdateCountDownTime(float deltaTime)
        {
            if (IsTimerPlaying == false)
                return;

            CurrentTime -= deltaTime;

            string timeText = ConvertTimeText(CurrentTime);
            onUpdateTimeText?.Invoke(timeText);

            if (CurrentTime > 0)
                return;

            CurrentTime = 0;
            IsTimerPlaying = false;
            onTimeUpCallback?.Invoke();
        }

        private string ConvertTimeText(float currentTime)
        {
            int minute = (int)currentTime / 60;
            int second = (int)currentTime % 60;
            return $"{minute:00}:{second:00}";
        }
    }
}