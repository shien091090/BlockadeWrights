using System;

namespace GameCore
{
    public class FortressModel : IFortressModel
    {
        public float CurrentHp => hpModel.CurrentHp;
        private readonly HealthPointModel hpModel;

        public FortressModel(float mapHp)
        {
            hpModel = new HealthPointModel(mapHp);
        }

        public event Action OnFortressDestroy;

        public void Damage()
        {
            hpModel.Damage(1);

            if (hpModel.IsDead)
                OnFortressDestroy?.Invoke();
        }

        public void Bind(IFortressView fortressView)
        {
            fortressView.SetDestroyHintActive(false);
            fortressView.GetHealthPointView.BindModel(hpModel);
        }
    }
}