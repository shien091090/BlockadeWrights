using UnityEngine;

namespace GameCore
{
    public interface IPlayerView : IFaceDirectionView
    {
        ITransform GetTransform { get; }
        float MoveSpeed { get; }
        Vector2 TouchRange { get; }
        void SetCellHintActive(bool isActive);
        void SetCellHintPosition(Vector2 pos);
    }
}