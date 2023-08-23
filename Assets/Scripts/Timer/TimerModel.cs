using System;

namespace GameCore
{
    public class TimerModel
    {
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

            if (CurrentTime > 0)
                return;

            CurrentTime = 0;
            IsTimerPlaying = false;
            onTimeUpCallback?.Invoke();
        }
    }
}