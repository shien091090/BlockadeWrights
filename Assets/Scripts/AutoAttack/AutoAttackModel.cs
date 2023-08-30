using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class AutoAttackModel
    {
        private readonly float attackFrequency;
        private readonly Vector2 position;
        private float timer;
        private float attackPower;
        public List<IAttackTarget> AttackTargets { get; }

        public AutoAttackModel(int attackRange, float attackFrequency, Vector2 position, float attackPower)
        {
            this.attackFrequency = attackFrequency;
            this.position = position;
            this.attackPower = attackPower;
            AttackTargets = new List<IAttackTarget>();
            timer = 0;
        }

        public void UpdateAttackTimer(float deltaTime)
        {
            timer += deltaTime;

            if (timer < attackFrequency || attackFrequency <= 0)
                return;

            timer = 0;
            IAttackTarget nearestTarget = CheckNearestTarget();
            nearestTarget?.Damage(attackPower);
        }

        public void AddAttackTarget(IAttackTarget attackTarget)
        {
            AttackTargets.Add(attackTarget);
        }

        private IAttackTarget CheckNearestTarget()
        {
            IAttackTarget nearestTarget = null;
            float nearestDistance = float.MaxValue;

            foreach (IAttackTarget target in AttackTargets)
            {
                Vector2 targetPos = target.GetPos;
                float distance = Vector2.Distance(targetPos, position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTarget = target;
                }
            }

            return nearestTarget;
        }
    }
}