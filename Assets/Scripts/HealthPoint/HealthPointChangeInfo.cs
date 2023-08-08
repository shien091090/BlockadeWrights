namespace GameCore
{
    public class HealthPointChangeInfo
    {
        public float CurrentHealthPointRate { get; }

        public HealthPointChangeInfo(float hpRate)
        {
            CurrentHealthPointRate = hpRate;
        }
    }
}