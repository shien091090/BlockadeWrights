using UnityEngine;

namespace GameCore
{
    public interface IDirectionStrategy
    {
        FaceDirectionState GetFaceDirection(FaceDirectionState originFaceDirectionState, Vector2 moveVector);
    }
}