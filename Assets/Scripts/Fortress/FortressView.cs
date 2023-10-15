using UnityEngine;

namespace GameCore
{
    public class FortressView : MonoBehaviour, IFortressView
    {
        [SerializeField] private GameObject go_destroyHint;

        public IHealthPointView GetHealthPointView { get; private set; }

        public void SetDestroyHintActive(bool isDestroy)
        {
            go_destroyHint.SetActive(isDestroy);
        }

        public void BindModel(IFortressModel fortressModel)
        {
            fortressModel.Bind(this);
        }

        private void Awake()
        {
            GetHealthPointView = GetComponent<HealthPointComponent>();
        }
    }
}