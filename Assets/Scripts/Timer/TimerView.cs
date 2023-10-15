using TMPro;
using UnityEngine;

namespace GameCore
{
    public class TimerView : MonoBehaviour, ITimerView
    {
        [SerializeField] private TextMeshProUGUI tmp_timer;

        private TimerModel timerModel;

        public void SetTimeText(string timeText)
        {
            tmp_timer.text = timeText;
        }

        public void SetTimerActive(bool isActive)
        {
            tmp_timer.gameObject.SetActive(isActive);
        }

        public void BindModel(TimerModel timerModel)
        {
            timerModel.Bind(this);
        }

        private void Update()
        {
            timerModel.Update();
        }
    }
}