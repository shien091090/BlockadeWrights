using System;

namespace GameCore
{
    public class FortressModel : IFortressModel
    {
        public event Action OnFortressDestroy;
        public HealthPointModel HpModel { get; }

        public bool IsInValid => HpModel.IsInValid;
        public float CurrentHp => HpModel.CurrentHp;

        public FortressModel(float mapHp)
        {
            HpModel = new HealthPointModel(mapHp);
        }

        public void Damage()
        {
            HpModel.Damage(1);

            if (HpModel.IsDead)
                OnFortressDestroy?.Invoke();
        }
    }
}