using UnityEngine;

namespace GameCore
{
    public class MovementProcessor
    {
        public Vector2 GetMoveVector(Vector2 normalizeDir, float speed, float deltaTime)
        {
            return normalizeDir * speed * deltaTime;
        }
    }
}