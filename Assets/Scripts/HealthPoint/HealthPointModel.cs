using System;

namespace GameCore
{
    public class HealthPointModel
    {
        private readonly float maxHp;
        public event Action<HealthPointChangeInfo> OnRefreshHealthPoint;

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
            OnRefreshHealthPoint?.Invoke(new HealthPointChangeInfo(CurrentHp / maxHp));
        }

        public void Heal(int healValue)
        {
            CurrentHp = Math.Min(maxHp, CurrentHp + healValue);
            OnRefreshHealthPoint?.Invoke(new HealthPointChangeInfo(CurrentHp / maxHp));
        }
    }
}