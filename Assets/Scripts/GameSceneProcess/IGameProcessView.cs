namespace GameCore
{
    public interface IGameProcessView
    {
        ITimerView GetTimerView { get; }
        IWaveHintView GetWaveHintView { get; }
        IFortressView GetFortressView { get; }
        void SetGameOverPanelActive(bool isActive);
        IMonsterView SpawnMonsterView(IMonsterModel monsterModel);
    }
}