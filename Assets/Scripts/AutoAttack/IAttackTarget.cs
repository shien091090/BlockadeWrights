using UnityEngine;

namespace GameCore
{
    public interface IAttackTarget
    {
        Vector2 GetPos { get; }
        string Id { get; }
        void Damage(float damageValue);
    }
}