using UnityEngine;

namespace GameCore
{
    public class PlayerModel
    {
        private readonly IInputAxisController inputAxisController;
        public FaceDirection PlayerFaceDir { get; private set; }

        public PlayerModel(IInputAxisController inputAxisController)
        {
            this.inputAxisController = inputAxisController;
            PlayerFaceDir = FaceDirection.Down;
        }

        public Vector2 UpdateMove(float speed, float deltaTime)
        {
            Vector2 moveVector = new Vector2(inputAxisController.GetHorizontalAxis(), inputAxisController.GetVerticalAxis()) * speed * deltaTime;
            PlayerFaceDir = GetFaceDirection(moveVector);
            return moveVector;
        }

        private FaceDirection GetFaceDirection(Vector2 moveVector)
        {
            if (moveVector.y > 0 && moveVector.x == 0)
                return FaceDirection.Up;
            else if (moveVector.y > 0 && moveVector.x > 0)
                return FaceDirection.UpAndRight;
            else if (moveVector.y == 0 && moveVector.x > 0)
                return FaceDirection.Right;
            else if (moveVector.y < 0 && moveVector.x > 0)
                return FaceDirection.RightAndDown;
            else if (moveVector.y < 0 && moveVector.x == 0)
                return FaceDirection.Down;
            else if (moveVector.y < 0 && moveVector.x < 0)
                return FaceDirection.DownAndLeft;
            else if (moveVector.y == 0 && moveVector.x < 0)
                return FaceDirection.Left;
            else if (moveVector.y > 0 && moveVector.x < 0)
                return FaceDirection.LeftAndUp;
            else
                return FaceDirection.Down;
        }
    }
}