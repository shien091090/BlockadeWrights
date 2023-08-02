namespace GameCore
{
    public class HealthPointModel
    {
        private readonly float maxHp;

        public bool IsValid => false;
        public float CurrentHp { private set; get; }

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