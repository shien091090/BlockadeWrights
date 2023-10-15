using TMPro;
using UnityEngine;
using Zenject;

namespace GameCore
{
    public class TimerView : MonoBehaviour, ITimerView
    {
        [SerializeField] private TextMeshProUGUI tmp_timer;

        [Inject] private ITimeManager timeManager;

        private TimerModel timerModel;

        public void SetTimeText(string timeText)
        {
            tmp_timer.text = timeText;
        }

        public void SetTimerActive(bool isActive)
        {
            tmp_timer.gameObject.SetActive(isActive);
        }

        private void Update()
        {
            timerModel.Update();
        }

        private void Awake()
        {
            timerModel = new TimerModel(timeManager);
            timerModel.Bind(this);
        }
    }
}