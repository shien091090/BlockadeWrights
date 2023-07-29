using UnityEngine;

namespace GameCore
{
    public interface IInGameMapModel
    {
        InGameMapCell GetCellInfo(Vector2 pos, FaceDirectionState faceDir, Vector2 touchRange = default);
    }
}