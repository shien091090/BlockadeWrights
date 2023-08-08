using UnityEngine;
using UnityEngine.UI;

namespace GameCore
{
    public class HealthPointComponent : MonoBehaviour
    {
        [SerializeField] private Slider sld_healthPoint;
        
        private HealthPointModel healthPointModel;

        public void Setup(HealthPointModel healthPointModel)
        {
            this.healthPointModel = healthPointModel;
            SetEventRegister();
        }

        private void SetEventRegister()
        {
            healthPointModel.OnRefreshHealthPoint -= RefreshHealthPointSlider;
            healthPointModel.OnRefreshHealthPoint += RefreshHealthPointSlider;
        }

        private void RefreshHealthPointSlider(HealthPointChangeInfo healthPointChangeInfo)
        {
            sld_healthPoint.value = healthPointChangeInfo.CurrentHealthPointRate;
        }
    }
}