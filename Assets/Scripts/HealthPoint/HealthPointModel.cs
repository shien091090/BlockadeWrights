using System;

namespace GameCore
{
    public class HealthPointModel
    {
        private readonly float maxHp;

        private IHealthPointView healthPointView;

        public bool IsInValid => maxHp <= 0;
        public float CurrentHp { private set; get; }
        public bool IsDead => CurrentHp == 0;

        public HealthPointModel(float maxHp)
        {
            this.maxHp = maxHp;
            CurrentHp = maxHp;
        }

        public void Damage(float damageValue)
        {
            CurrentHp = Math.Max(0, CurrentHp - damageValue);
            healthPointView?.RefreshHealthPointSlider(GetCurrentHealthPointChangeInfo());
        }

        public void Heal(int healValue)
        {
            CurrentHp = Math.Min(maxHp, CurrentHp + healValue);
            healthPointView?.RefreshHealthPointSlider(GetCurrentHealthPointChangeInfo());
        }

        public bool WellDieWhenDamage(float damageValue)
        {
            return CurrentHp - damageValue <= 0;
        }

        public void Bind(IHealthPointView healthPointView)
        {
            this.healthPointView = healthPointView;
            healthPointView?.RefreshHealthPointSlider(GetCurrentHealthPointChangeInfo());
        }

        private HealthPointChangeInfo GetCurrentHealthPointChangeInfo()
        {
            return new HealthPointChangeInfo(CurrentHp / maxHp);
        }
    }
}