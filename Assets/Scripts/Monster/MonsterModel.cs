using System;
using UnityEngine;

namespace GameCore
{
    public class MonsterModel : IMonsterModel
    {
        public ITransform GetTransform => monsterView.GetTransform;
        public string Id => monsterView.GetId;


        public Vector2 GetStartPoint => path != null && path.IsEmpty == false ?
            path.GetPoint(0) :
            Vector2.zero;

        public float GetHp => hpModel.CurrentHp;

        public EntityState GetEntityState
        {
            get
            {
                if (hpModel == null || hpModel.IsDead)
                {
                    entityState = EntityState.Dead;
                    return EntityState.Dead;
                }

                return entityState;
            }
        }

        private readonly FaceDirection lookFaceDirection;
        private readonly HealthPointModel hpModel;
        private readonly MonsterMovementPath path;
        private readonly ITimeManager timeAdapter;
        private EntityState entityState;
        private IMonsterView monsterView;
        private IFortressModel fortressModel;

        public int CurrentTargetPathIndex { get; private set; }
        public bool IsArrivedGoal => !path.IsEmpty && CurrentTargetPathIndex > path.GetLastPointIndex;

        private float MoveSpeed { get; }
        private Sprite GetFrontSideSprite { get; }
        private Sprite GetBackSideSprite { get; }

        public MonsterModel(MonsterMovementPath path, IMonsterSetting setting, ITimeManager timeAdapter)
        {
            this.path = path;
            this.timeAdapter = timeAdapter;

            if (path.IsEmpty == false)
                CurrentTargetPathIndex = 1;

            MoveSpeed = setting.GetMoveSpeed;
            lookFaceDirection = new FaceDirection(new QuadrantDirectionStrategy(), FaceDirectionState.DownAndRight);
            hpModel = new HealthPointModel(setting.GetHp);
            GetFrontSideSprite = setting.GetFrontSideSprite;
            GetBackSideSprite = setting.GetBackSideSprite;
        }

        public void Update()
        {
            Vector2 translationVector = UpdateMove(monsterView.GetTransform.Position, MoveSpeed, timeAdapter.DeltaTime);
            if (translationVector != default)
                monsterView.GetTransform.Translate(translationVector);
        }

        public void SetAttackTarget(IFortressModel fortressModel)
        {
            this.fortressModel = fortressModel;
        }

        public void Damage(float damageValue)
        {
            hpModel.Damage(damageValue);

            if (hpModel.IsDead)
            {
                entityState = EntityState.Dead;
                monsterView.SetActive(false);
            }
        }

        public void PreDamage(float damageValue)
        {
            if (hpModel.WellDieWhenDamage(damageValue) && hpModel.IsDead == false)
                entityState = EntityState.PreDie;
        }

        public void Bind(IMonsterView view)
        {
            monsterView = view;
            entityState = EntityState.Normal;
            
            monsterView.SetupHp(hpModel);
            monsterView.InitSprite(GetFrontSideSprite, GetBackSideSprite);
            lookFaceDirection.BindView(monsterView);

            monsterView.Bind(this);
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

        private Vector2 UpdateMove(Vector2 currentPos, float speed, float deltaTime)
        {
            if (path.IsEmpty || IsArrivedGoal)
                return Vector2.zero;

            Vector2 end = path.GetPoint(CurrentTargetPathIndex);
            Vector2 moveVector = (end - currentPos).normalized * speed * deltaTime;
            lookFaceDirection.MoveToChangeFaceDirection(moveVector);

            if (IsArriveTarget(currentPos, end, moveVector) == false)
                return moveVector;

            CurrentTargetPathIndex++;

            if (IsArrivedGoal)
                fortressModel?.Damage();

            return moveVector;
        }
    }
}