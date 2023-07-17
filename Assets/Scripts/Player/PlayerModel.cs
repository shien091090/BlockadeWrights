using UnityEngine;

namespace GameCore
{
    public class PlayerModel
    {
        public Vector2 UpdateMove(Vector2 dir, float speed, float deltaTime)
        {
            return dir * speed * deltaTime;
        }
    }
}