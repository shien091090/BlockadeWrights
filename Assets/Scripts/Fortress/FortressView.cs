using UnityEngine;

namespace GameCore
{
    public class FortressView : MonoBehaviour
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

        public void Init(FortressModel fortressModel)
        {
            HpComponent.BindModel(fortressModel.HpModel);
            SetDestroyHintActive(false);
        }

        public void SetDestroyHintActive(bool isDestroy)
        {
            go_destroyHint.SetActive(isDestroy);
        }
    }
}