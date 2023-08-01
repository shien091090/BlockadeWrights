using UnityEngine;

namespace GameCore
{
    public class MonsterModel
    {
        private readonly MonsterMovementPath path;

        public MonsterModel(MonsterMovementPath path)
        {
            this.path = path;
        }

        public Vector2 UpdateMove(int speed, int deltaTime)
        {
            if (path.IsEmpty)
                return Vector2.zero;
            else
            {
                Vector2 start = path.GetPoint(0);
                Vector2 end = path.GetPoint(1);
                return (end - start).normalized * speed * deltaTime;
            }
        }
    }
}