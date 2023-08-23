using System;

namespace GameCore
{
    public class TimerModel
    {
        private Action onTimeUpCallback;
        public event Action<string> onUpdateTimeText;
        public bool IsTimerPlaying { get; private set; }
        public float CurrentTime { get; private set; }
        public string CurrentTimeText => ConvertTimeText(CurrentTime);

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
            int second = (int)Math.Ceiling(currentTime % 60f);
            return $"{minute:00}:{second:00}";
        }
    }
}