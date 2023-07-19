using System;
using UnityEngine;

namespace GameCore
{
    public class PlayerModel
    {
        private readonly IInputAxisController inputAxisController;
        public event Action<FaceDirection> OnFaceDirectionChanged;
        public FaceDirection CurrentFaceDirection { get; private set; }

        public PlayerModel(IInputAxisController inputAxisController)
        {
            this.inputAxisController = inputAxisController;
            CurrentFaceDirection = FaceDirection.DownAndRight;
        }

        public Vector2 UpdateMove(float speed, float deltaTime)
        {
            Vector2 moveVector = new Vector2(inputAxisController.GetHorizontalAxis(), inputAxisController.GetVerticalAxis()) * speed * deltaTime;
            CheckChangeFaceDirection(moveVector);

            return moveVector;
        }

        private FaceDirection GetFaceDirection(FaceDirection originFaceDirection, Vector2 moveVector)
        {
            Vector2 currentFaceVector = originFaceDirection switch
            {
                FaceDirection.UpAndRight => new Vector2(1, 1),
                FaceDirection.UpAndLeft => new Vector2(-1, 1),
                FaceDirection.DownAndRight => new Vector2(1, -1),
                FaceDirection.DownAndLeft => new Vector2(-1, -1),
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
                return FaceDirection.UpAndRight;
            else if (currentFaceVector.x == 1 && currentFaceVector.y == -1)
                return FaceDirection.DownAndRight;
            else if (currentFaceVector.x == -1 && currentFaceVector.y == 1)
                return FaceDirection.UpAndLeft;
            else if (currentFaceVector.x == -1 && currentFaceVector.y == -1)
                return FaceDirection.DownAndLeft;
            else
                return FaceDirection.DownAndRight;
        }

        private void CheckChangeFaceDirection(Vector2 moveVector)
        {
            FaceDirection afterFaceDirection = GetFaceDirection(CurrentFaceDirection, moveVector);
            if (afterFaceDirection == CurrentFaceDirection)
                return;

            CurrentFaceDirection = afterFaceDirection;
            OnFaceDirectionChanged?.Invoke(CurrentFaceDirection);
        }
    }
}