namespace GameCore
{
    public interface IHealthPointView
    {
        void RefreshHealthPointSlider(HealthPointChangeInfo healthPointChangeInfo);
        void BindModel(HealthPointModel healthPointModel);
    }
}