using UnityEngine;

namespace GameCore
{
    public class MonsterModel
    {
        private readonly MonsterMovementPath path;
        private int currentTargetPathIndex;

        public MonsterModel(MonsterMovementPath path)
        {
            this.path = path;

            if (path.IsEmpty == false)
                currentTargetPathIndex = 1;
        }

        public Vector2 UpdateMove(Vector2 currentPos, int speed, int deltaTime)
        {
            if (path.IsEmpty)
                return Vector2.zero;
            else
            {
                Vector2 end = path.GetPoint(currentTargetPathIndex);
                return (end - currentPos).normalized * speed * deltaTime;
            }
        }

        public void SetTargetPathIndex(int index)
        {
            currentTargetPathIndex = index;
        }
    }
}