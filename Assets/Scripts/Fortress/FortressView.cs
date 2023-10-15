using UnityEngine;

namespace GameCore
{
    public class FortressView : MonoBehaviour, IFortressView
    {
        [SerializeField] private GameObject go_destroyHint;

        private HealthPointComponent hpComponent;

        private HealthPointComponent HpComponent
        {
            get
            {
                if (hpComponent == null)
                    hpComponent = GetComponent<HealthPointComponent>();

                return hpComponent;
            }
        }

        public void SetDestroyHintActive(bool isDestroy)
        {
            go_destroyHint.SetActive(isDestroy);
        }

        public void BindModel(IFortressModel fortressModel)
        {
            // HpComponent.BindModel(fortressModel.HpModel);
            // SetDestroyHintActive(false);
        }
    }
}