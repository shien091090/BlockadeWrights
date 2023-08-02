namespace GameCore
{
    public class HealthPointModel
    {
        private readonly float maxHp;

        public bool IsValid => maxHp <= 0;
        public float CurrentHp { private set; get; }
        public bool IsDead => CurrentHp == 0;

        public HealthPointModel(float maxHp)
        {
            this.maxHp = maxHp;
            CurrentHp = maxHp;
        }

        public void Damage(float damageValue)
        {
            CurrentHp -= damageValue;
        }
    }
}