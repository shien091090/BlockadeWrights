using UnityEngine;

namespace GameCore
{
    public interface IMonsterModel : IAttackTarget
    {
        Vector2 GetStartPoint { get; }
        float GetHp { get; }
        void Update();
        void SetAttackTarget(IFortressModel fortressModel);
        void Bind(IMonsterView monsterView);
    }
}