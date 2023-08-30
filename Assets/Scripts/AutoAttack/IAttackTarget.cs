using UnityEngine;

namespace GameCore
{
    public interface IAttackTarget
    {
        Vector2 GetPos { get; }
        void Damage(float damageValue);
    }
}