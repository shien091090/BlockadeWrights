using System;

namespace GameCore
{
    public class FortressModel : IFortressModel
    {
        public float CurrentHp => hpModel.CurrentHp;
        private readonly HealthPointModel hpModel;
        private IFortressView fortressView;

        public FortressModel(float mapHp)
        {
            hpModel = new HealthPointModel(mapHp);
        }

        public event Action OnFortressDestroy;

        public void Damage()
        {
            hpModel.Damage(1);

            if (hpModel.IsDead)
            {
                fortressView?.SetDestroyHintActive(true);
                OnFortressDestroy?.Invoke();
            }
        }

        public void Bind(IFortressView fortressView)
        {
            this.fortressView = fortressView;
            fortressView.SetDestroyHintActive(false);
            fortressView.GetHealthPointView.BindModel(hpModel);
        }
    }
}