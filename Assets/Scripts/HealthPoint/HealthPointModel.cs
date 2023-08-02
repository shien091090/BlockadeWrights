namespace GameCore
{
    public class HealthPointModel
    {
        private readonly float maxHp;

        public HealthPointModel(int maxHp)
        {
            this.maxHp = maxHp;
        }

        public bool IsValid => false;
    }
}