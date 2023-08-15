namespace GameCore
{
    public class FortressModel
    {
        private readonly HealthPointModel hpModel;

        public bool IsInValid => hpModel.IsInValid;

        public FortressModel(float mapHp)
        {
            hpModel = new HealthPointModel(mapHp);
        }

        public void Damage(float damageValue)
        {
            hpModel.Damage(damageValue);
        }
    }
}