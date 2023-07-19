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
            PlayerFaceDir = FaceDirection.DownAndRight;
        }

        public Vector2 UpdateMove(float speed, float deltaTime)
        {
            Vector2 moveVector = new Vector2(inputAxisController.GetHorizontalAxis(), inputAxisController.GetVerticalAxis()) * speed * deltaTime;
            PlayerFaceDir = GetFaceDirection(moveVector);
            return moveVector;
        }

        private FaceDirection GetFaceDirection(Vector2 moveVector)
        {
            if (moveVector.x > 0 && moveVector.y > 0)
                return FaceDirection.UpAndRight;
            else if (moveVector.x < 0 && moveVector.y > 0)
                return FaceDirection.UpAndLeft;

            return FaceDirection.DownAndRight;
        }
    }
}