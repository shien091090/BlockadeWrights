namespace GameCore
{
    public interface IGameProcessView
    {
        ITimerView GetTimerView { get; }
        IWaveHintView GetWaveHintView { get; }
        IFortressView GetFortressView { get; }
        IMonsterView SpawnMonsterView(IMonsterModel monsterModel);
    }
}