using System;
using UnityEngine;

namespace GameCore
{
    public interface IMonsterModel : IAttackTarget
    {
        event Action OnDead;
        event Action OnArrivedGoal;
        Vector2 GetStartPoint { get; }
        float GetHp { get; }
        void Update();
        void SetAttackTarget(IFortressModel fortressModel);
        void Bind(IMonsterView monsterView);
    }
}