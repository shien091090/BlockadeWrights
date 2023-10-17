using System;

namespace GameCore
{
    public interface IMonsterSpawner
    {
        event Action<IMonsterModel> OnSpawnMonster;
        event Action OnStartNextWave;
        string GetWaveHint { get; }
        int TotalMonsterCount { get; }
        bool IsNeedCountDownToSpawnMonster();
        float GetStartTimeSeconds();
        void CheckUpdateSpawn(float deltaTime);
    }
}