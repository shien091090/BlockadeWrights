using System;
using UnityEngine;

namespace GameCore
{
    public class FaceDirection
    {
        private IFaceDirectionView faceDirectionView;
        private readonly IDirectionStrategy directionStrategy;

        public FaceDirectionState CurrentFaceDirectionState { get; private set; }

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
            faceDirectionView?.RefreshFaceDirection(CurrentFaceDirectionState);
        }

        public void BindView(IFaceDirectionView faceDirectionView)
        {
            this.faceDirectionView = faceDirectionView;
        }
    }
}