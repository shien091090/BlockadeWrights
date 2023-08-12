using System;
using UnityEngine;

namespace GameCore
{
    public class MonsterModel
    {
        private readonly MonsterMovementPath path;

        public event Action OnDamageFort;
        public event Action OnDead;

        public FaceDirection LookFaceDirection { get; }
        public HealthPointModel HealthPointModel { get; private set; }
        public int CurrentTargetPathIndex { get; private set; }
        public bool IsArrivedGoal => !path.IsEmpty && CurrentTargetPathIndex > path.GetLastPointIndex;
        public bool IsDead => HealthPointModel != null && HealthPointModel.IsDead;

        public Vector2 GetStartPoint => path != null && path.IsEmpty == false ?
            path.GetPoint(0) :
            Vector2.zero;

        public MonsterModel(MonsterMovementPath path)
        {
            this.path = path;

            if (path.IsEmpty == false)
                CurrentTargetPathIndex = 1;

            LookFaceDirection = new FaceDirection(new QuadrantDirectionStrategy(), FaceDirectionState.DownAndRight);
        }

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

        public void InitHp(float maxHp)
        {
            HealthPointModel = new HealthPointModel(maxHp);
        }

        public void SetTargetPathIndex(int index)
        {
            CurrentTargetPathIndex = index;
        }

        public void Damage(float damageValue)
        {
            if (HealthPointModel == null)
                return;

            HealthPointModel.Damage(damageValue);

            if (HealthPointModel.IsDead)
                OnDead?.Invoke();
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