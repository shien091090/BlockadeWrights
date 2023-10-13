using UnityEngine;

namespace GameCore
{
    public interface IPlayerView : IFaceDirectionView
    {
        ITransform GetTransform { get; }
        void SetCellHintActive(bool isActive);
        void SetCellHintPosition(Vector2 pos);
    }
}