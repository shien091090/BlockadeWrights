using System;
using UnityEngine;

namespace GameCore
{
    public class FaceDirection
    {
        public event Action<FaceDirectionState> OnFaceDirectionChanged;
        public FaceDirectionState CurrentFaceDirectionState { get; private set; }
        private IDirectionStrategy directionStrategy { get; }

        public FaceDirection(IDirectionStrategy directionStrategy, FaceDirectionState startFaceDir = FaceDirectionState.None)
        {
            CurrentFaceDirectionState = startFaceDir;
            this.directionStrategy = directionStrategy;
        }

        public void MoveToChangeFaceDirection(Vector2 moveVector)
        {
            FaceDirectionState afterFaceDirectionState = directionStrategy.GetFaceDirection(CurrentFaceDirectionState, moveVector);
            if (afterFaceDirectionState == CurrentFaceDirectionState)
                return;

            CurrentFaceDirectionState = afterFaceDirectionState;
            OnFaceDirectionChanged?.Invoke(CurrentFaceDirectionState);
        }
    }
}