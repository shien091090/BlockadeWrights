using UnityEngine;

namespace GameCore
{
    public class TransformComponent : MonoBehaviour, ITransform
    {
        public Vector2 Position => transform.position;

        public void Translate(Vector2 translationVector)
        {
            transform.Translate(translationVector);
        }
    }
}