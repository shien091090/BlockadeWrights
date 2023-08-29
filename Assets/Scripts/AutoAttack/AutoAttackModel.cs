using System.Collections.Generic;

namespace GameCore
{
    public class AutoAttackModel
    {
        public List<IAttackTarget> AttackTargets { get; }

        public AutoAttackModel(int attackRange, int attackFrequency)
        {
            AttackTargets = new List<IAttackTarget>();
        }

        public void UpdateAttackTimer(int deltaTime)
        {
        }
    }
}