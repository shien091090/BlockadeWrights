namespace GameCore
{
    public class HealthPointModel
    {
        private readonly float maxHp;

        public bool IsValid => false;
        public float CurrentHp => 5;

        public HealthPointModel(int maxHp)
        {
            this.maxHp = maxHp;
        }

        public void Damage(float damageValue)
        {
        }
    }
}