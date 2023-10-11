using System;
using UnityEngine;

namespace GameCore
{
    public interface IMonsterModel : IAttackTarget
    {
        event Action OnDamageFort;
        Vector2 GetStartPoint { get; }
        void Update();
        void Bind(IMonsterView monsterView);
    }
}