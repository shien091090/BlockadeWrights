using UnityEngine;

namespace GameCore
{
    public class TransformComponent : MonoBehaviour, ITransform
    {
        public Vector2 Position => transform.position;
    }
}