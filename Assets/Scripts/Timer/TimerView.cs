using UnityEngine;

namespace GameCore
{
    public class TimerView : MonoBehaviour
    {
        private TimerModel timerModel;

        private void Update()
        {
            if (timerModel.IsTimerPlaying)
                timerModel.UpdateCountDownTime(Time.deltaTime);
        }
    }
}