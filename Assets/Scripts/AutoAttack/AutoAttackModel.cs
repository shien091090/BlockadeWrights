using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class AutoAttackModel : IColliderHandler
    {
        private readonly int attackRange;
        private readonly float attackFrequency;
        private readonly Vector2 position;
        private readonly float attackPower;
        private readonly IBuildingAttackView buildingAttackView;
        private float timer;

        public List<IAttackTarget> AttackTargets { get; }
        public bool IsStop { get; set; }

        public AutoAttackModel(int attackRange, float attackFrequency, Vector2 position, float attackPower, IBuildingAttackView buildingAttackView)
        {
            this.attackRange = attackRange;
            this.attackFrequency = attackFrequency;
            this.position = position;
            this.attackPower = attackPower;
            this.buildingAttackView = buildingAttackView;
            AttackTargets = new List<IAttackTarget>();
            timer = 0;
        }

        public void ColliderTriggerEnter(ITriggerCollider col)
        {
        }

        public void ColliderTriggerExit(ITriggerCollider col)
        {
        }

        public void ColliderTriggerStay(ITriggerCollider col)
        {
            IAttackTarget attackTarget = col.GetComponent<IAttackTarget>();
            if (attackTarget != null)
                AddAttackTarget(attackTarget);
        }

        public void CollisionEnter(ICollision col)
        {
        }

        public void CollisionExit(ICollision col)
        {
        }

        public void SetStopState(bool isStop)
        {
            IsStop = isStop;
        }

        public void UpdateAttackTimer(float deltaTime)
        {
            if (IsStop)
                return;

            timer += deltaTime;

            if (timer < attackFrequency || attackFrequency <= 0)
                return;

            timer = 0;

            CheckRemoveOverDistanceTarget();
            IAttackTarget nearestTarget = CheckNearestTarget();
            if (nearestTarget != null)
                buildingAttackView.LaunchAttack(nearestTarget, attackPower);

            if (nearestTarget != null && nearestTarget.IsDead)
                RemoveAttackTarget(nearestTarget);
        }

        private void CheckRemoveOverDistanceTarget()
        {
            for (int i = AttackTargets.Count - 1; i >= 0; i--)
            {
                IAttackTarget target = AttackTargets[i];
                if (Vector2.Distance(target.GetTransform.Position, position) > attackRange)
                    AttackTargets.RemoveAt(i);
            }
        }

        private IAttackTarget CheckNearestTarget()
        {
            IAttackTarget nearestTarget = null;
            float nearestDistance = float.MaxValue;

            foreach (IAttackTarget target in AttackTargets)
            {
                Vector2 targetPos = target.GetTransform.Position;
                float distance = Vector2.Distance(targetPos, position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTarget = target;
                }
            }

            return nearestTarget;
        }

        private void AddAttackTarget(IAttackTarget attackTarget)
        {
            if (AttackTargets.Exists(x => x.Id == attackTarget.Id) == false)
                AttackTargets.Add(attackTarget);
        }

        private void RemoveAttackTarget(IAttackTarget attackTarget)
        {
            AttackTargets.RemoveAll(x => x.Id == attackTarget.Id);
        }
    }
}