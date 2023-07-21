using System;
using UnityEngine;

namespace GameCore
{
    public class FaceDirection
    {
        public event Action<FaceDirectionState> OnFaceDirectionChanged;
        public FaceDirectionState CurrentFaceDirectionState { get; private set; }

        public FaceDirection()
        {
            CurrentFaceDirectionState = FaceDirectionState.DownAndRight;
        }

        public void CheckChangeFaceDirection(Vector2 moveVector)
        {
            FaceDirectionState afterFaceDirectionState = GetFaceDirection(CurrentFaceDirectionState, moveVector);
            if (afterFaceDirectionState == CurrentFaceDirectionState)
                return;

            CurrentFaceDirectionState = afterFaceDirectionState;
            OnFaceDirectionChanged?.Invoke(CurrentFaceDirectionState);
        }

        private FaceDirectionState GetFaceDirection(FaceDirectionState originFaceDirectionState, Vector2 moveVector)
        {
            Vector2 currentFaceVector = originFaceDirectionState switch
            {
                FaceDirectionState.UpAndRight => new Vector2(1, 1),
                FaceDirectionState.UpAndLeft => new Vector2(-1, 1),
                FaceDirectionState.DownAndRight => new Vector2(1, -1),
                FaceDirectionState.DownAndLeft => new Vector2(-1, -1),
                _ => Vector2.zero
            };

            float xAxisReverse = 1;
            if ((moveVector.x > 0 && currentFaceVector.x < 0) || (moveVector.x < 0 && currentFaceVector.x > 0))
                xAxisReverse = -1;

            float yAxisReverse = 1;
            if ((moveVector.y > 0 && currentFaceVector.y < 0) || (moveVector.y < 0 && currentFaceVector.y > 0))
                yAxisReverse = -1;

            Vector2 reverseVector = new Vector2(xAxisReverse, yAxisReverse);

            currentFaceVector *= reverseVector;
            if (currentFaceVector.x == 1 && currentFaceVector.y == 1)
                return FaceDirectionState.UpAndRight;
            else if (currentFaceVector.x == 1 && currentFaceVector.y == -1)
                return FaceDirectionState.DownAndRight;
            else if (currentFaceVector.x == -1 && currentFaceVector.y == 1)
                return FaceDirectionState.UpAndLeft;
            else if (currentFaceVector.x == -1 && currentFaceVector.y == -1)
                return FaceDirectionState.DownAndLeft;
            else
                return FaceDirectionState.DownAndRight;
        }
    }
}