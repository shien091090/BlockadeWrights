using UnityEngine;

namespace GameCore
{
    public interface IInGameMapCell
    {
        bool IsEmpty { get; }
        IntVector2 GridPosition { get; }
        Vector2 CenterPosition { get; }
    }
}