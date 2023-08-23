using TMPro;
using UnityEngine;

namespace GameCore
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tmp_timer;

        private TimerModel timerModel;

        private void Update()
        {
            if (timerModel.IsTimerPlaying)
                timerModel.UpdateCountDownTime(Time.deltaTime);
        }

        public void StartCountDown(float countDownSeconds)
        {
            timerModel.StartCountDown(countDownSeconds, OnTimeUp);
            SetTimerActive(true);
        }

        private void SetTimerActive(bool isActive)
        {
            tmp_timer.gameObject.SetActive(isActive);
        }

        private void SetTimeText(string timeText)
        {
            tmp_timer.text = timeText;
        }

        private void Awake()
        {
            timerModel = new TimerModel();

            RegisterEvent();
            SetTimerActive(false);
        }

        private void RegisterEvent()
        {
            timerModel.onUpdateTimeText -= SetTimeText;
            timerModel.onUpdateTimeText += SetTimeText;
        }

        private void OnTimeUp()
        {
            // SetTimerActive(false);
        }
    }
}