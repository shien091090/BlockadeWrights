namespace GameCore
{
    public interface IFortressView
    {
        IHealthPointView GetHealthPointView { get; }
        void SetDestroyHintActive(bool isDestroy);
        void BindModel(IFortressModel fortressModel);
    }
}