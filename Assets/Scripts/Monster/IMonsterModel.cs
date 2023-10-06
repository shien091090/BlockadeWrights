using System;
using UnityEngine;

namespace GameCore
{
    public interface IMonsterModel
    {
        event Action OnDamageFort;
        event Action OnDead;
        Vector2 GetStartPoint { get; }
        HealthPointModel HpModel { get; }
        FaceDirection LookFaceDirection { get; }
        float MoveSpeed { get; }
        Sprite GetFrontSideSprite { get; }
        Sprite GetBackSideSprite { get; }
        bool IsDead { get; }
        Vector2 UpdateMove(Vector2 currentPos, float speed, float deltaTime);
        void Damage(float damageValue);
    }
}