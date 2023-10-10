using System;
using UnityEngine;

namespace GameCore
{
    public class MonsterModel : IMonsterModel
    {
        public FaceDirection LookFaceDirection { get; }
        public HealthPointModel HpModel { get; }

        public Vector2 GetStartPoint => path != null && path.IsEmpty == false ?
            path.GetPoint(0) :
            Vector2.zero;

        public float MoveSpeed { get; }
        public Sprite GetFrontSideSprite { get; }
        public Sprite GetBackSideSprite { get; }

        public EntityState GetEntityState
        {
            get
            {
                if (HpModel == null || HpModel.IsDead)
                {
                    entityState = EntityState.Dead;
                    return EntityState.Dead;
                }

                return entityState;
            }
        }

        private readonly MonsterMovementPath path;
        private EntityState entityState;

        public int CurrentTargetPathIndex { get; private set; }
        public bool IsArrivedGoal => !path.IsEmpty && CurrentTargetPathIndex > path.GetLastPointIndex;

        public MonsterModel(MonsterMovementPath path, IMonsterSetting setting)
        {
            this.path = path;

            if (path.IsEmpty == false)
                CurrentTargetPathIndex = 1;

            MoveSpeed = setting.GetMoveSpeed;
            LookFaceDirection = new FaceDirection(new QuadrantDirectionStrategy(), FaceDirectionState.DownAndRight);
            HpModel = new HealthPointModel(setting.GetHp);
            GetFrontSideSprite = setting.GetFrontSideSprite;
            GetBackSideSprite = setting.GetBackSideSprite;
        }

        public event Action OnDamageFort;
        public event Action OnDead;

        public Vector2 UpdateMove(Vector2 currentPos, float speed, float deltaTime)
        {
            if (path.IsEmpty || IsArrivedGoal)
                return Vector2.zero;

            Vector2 end = path.GetPoint(CurrentTargetPathIndex);
            Vector2 moveVector = (end - currentPos).normalized * speed * deltaTime;
            LookFaceDirection.MoveToChangeFaceDirection(moveVector);

            if (IsArriveTarget(currentPos, end, moveVector) == false)
                return moveVector;

            CurrentTargetPathIndex++;

            if (IsArrivedGoal)
                OnDamageFort?.Invoke();

            return moveVector;
        }

        public void Damage(float damageValue)
        {
            HpModel.Damage(damageValue);

            if (HpModel.IsDead)
                OnDead?.Invoke();
        }

        public void PreDamage(float damageValue)
        {
            if (HpModel.WellDieWhenDamage(damageValue) && HpModel.IsDead == false)
                entityState = EntityState.PreDie;
        }

        public void SetTargetPathIndex(int index)
        {
            CurrentTargetPathIndex = index;
        }

        private bool IsArriveTarget(Vector2 currentPos, Vector2 end, Vector2 moveVector)
        {
            bool directionXIsRight = (end.x - currentPos.x) > 0;
            bool directionYIsUp = (end.y - currentPos.y) > 0;

            bool arriveX = directionXIsRight ?
                currentPos.x + moveVector.x >= end.x :
                currentPos.x + moveVector.x <= end.x;

            bool arriveY = directionYIsUp ?
                currentPos.y + moveVector.y >= end.y :
                currentPos.y + moveVector.y <= end.y;

            return arriveX && arriveY;
        }
    }
}