using UnityEngine;

namespace GameCore
{
    public interface ITransform
    {
        Vector2 Position { get; }
        void Translate(Vector2 translationVector);
    }
}