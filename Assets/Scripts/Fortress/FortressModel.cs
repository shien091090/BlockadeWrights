using System;

namespace GameCore
{
    public class FortressModel
    {
        private readonly HealthPointModel hpModel;

        public event Action OnFortressDestroy;

        public bool IsInValid => hpModel.IsInValid;
        public float CurrentHp => hpModel.CurrentHp;

        public FortressModel(float mapHp)
        {
            hpModel = new HealthPointModel(mapHp);
        }

        public void Damage(float damageValue)
        {
            hpModel.Damage(damageValue);

            if (hpModel.IsDead)
                OnFortressDestroy?.Invoke();
        }
    }
}