using System;

namespace GameCore
{
    public interface IMonsterSpawner
    {
        event Action<IMonsterModel> OnSpawnMonster;
    }
}