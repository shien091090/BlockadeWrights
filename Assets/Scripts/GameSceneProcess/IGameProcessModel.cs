namespace GameCore
{
    public interface IGameProcessModel
    {
        void Update();
        void StartGame();
        void Bind(IGameProcessView gameProcessView);
    }
}