using UnityEngine;
using UnityEngine.UI;

namespace GameCore
{
    public class HealthPointComponent : MonoBehaviour, IHealthPointView
    {
        [SerializeField] private Slider sld_healthPoint;

        public void RefreshHealthPointSlider(HealthPointChangeInfo healthPointChangeInfo)
        {
            sld_healthPoint.value = healthPointChangeInfo.CurrentHealthPointRate;
        }

        public void BindModel(HealthPointModel healthPointModel)
        {
            healthPointModel.Bind(this);
        }
    }
}