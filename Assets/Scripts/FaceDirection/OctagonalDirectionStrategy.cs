using UnityEngine;

namespace GameCore
{
    public class OctagonalDirectionStrategy : IDirectionStrategy
    {
        public FaceDirectionState GetFaceDirection(FaceDirectionState originFaceDirectionState, Vector2 moveVector)
        {
            if (moveVector.x == 0 && moveVector.y == 0)
                return originFaceDirectionState;

            if (moveVector.x == 0 && moveVector.y > 0)
                return FaceDirectionState.Up;
            else if(moveVector.x == 0 && moveVector.y < 0)
                return FaceDirectionState.Down;
            else if (moveVector.x > 0 && moveVector.y == 0)
                return FaceDirectionState.Right;
            else if (moveVector.x < 0 && moveVector.y == 0)
                return FaceDirectionState.Left;
            else if (moveVector.x > 0 && moveVector.y > 0)
                return FaceDirectionState.UpAndRight;
            else if(moveVector.x > 0 && moveVector.y < 0)
                return FaceDirectionState.DownAndRight;
            else if (moveVector.x < 0 && moveVector.y > 0)
                return FaceDirectionState.UpAndLeft;
            else if (moveVector.x < 0 && moveVector.y < 0)
                return FaceDirectionState.DownAndLeft;
            else
                return FaceDirectionState.None;
        }
    }
}