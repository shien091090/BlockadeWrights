using System;

namespace GameCore
{
    public interface IMonsterSpawner
    {
        event Action<IMonsterModel> OnSpawnMonster;
        string GetWaveHint { get; }
        void CheckUpdateSpawn(float deltaTime);
        event Action OnStartNextWave;
        bool IsNeedCountDownToSpawnMonster();
        float GetStartTimeSeconds();
    }
}