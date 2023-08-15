using System;

namespace GameCore
{
    public class FortressModel
    {
        public HealthPointModel HpModel { get; }

        public event Action OnFortressDestroy;

        public bool IsInValid => HpModel.IsInValid;
        public float CurrentHp => HpModel.CurrentHp;

        public FortressModel(float mapHp)
        {
            HpModel = new HealthPointModel(mapHp);
        }

        public void Damage(float damageValue)
        {
            HpModel.Damage(damageValue);

            if (HpModel.IsDead)
                OnFortressDestroy?.Invoke();
        }
    }
}