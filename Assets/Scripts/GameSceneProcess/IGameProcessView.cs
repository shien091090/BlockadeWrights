namespace GameCore
{
    public interface IGameProcessView
    {
        ITimerView GetTimerView { get; }
        IWaveHintView GetWaveHintView { get; }
        IMonsterView SpawnMonsterView(IMonsterModel monsterModel);
    }
}