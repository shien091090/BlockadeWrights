namespace GameCore
{
    public interface IFortressView
    {
        void BindModel(IFortressModel fortressModel);
        void SetDestroyHintActive(bool isDestroy);
    }
}