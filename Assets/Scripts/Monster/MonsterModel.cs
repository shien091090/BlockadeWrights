using UnityEngine;

namespace GameCore
{
    public class MonsterModel
    {
        private MonsterMovementPath Path { get; }

        public MonsterModel(MonsterMovementPath path)
        {
            Path = path;
        }

        public Vector2 UpdateMove(int speed, int deltaTime)
        {
            return Vector2.zero;
        }
    }
}