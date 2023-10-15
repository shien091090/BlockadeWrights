using System;

namespace GameCore
{
    public class FortressModel : IFortressModel
    {
        public float CurrentHp => HpModel.CurrentHp;
        public HealthPointModel HpModel { get; }
        public bool IsInValid => HpModel.IsInValid;

        public FortressModel(float mapHp)
        {
            HpModel = new HealthPointModel(mapHp);
        }

        public event Action OnFortressDestroy;

        public void Damage()
        {
            HpModel.Damage(1);

            if (HpModel.IsDead)
                OnFortressDestroy?.Invoke();
        }

        public void Bind(IFortressView fortressView)
        {
            fortressView.SetDestroyHintActive(false);
            fortressView.GetHealthPointView.BindModel(HpModel);
        }
    }
}