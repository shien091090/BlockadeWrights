using UnityEngine;

namespace GameCore
{
    public class PlayerModel
    {
        private readonly IInputAxisController inputAxisController;
        public FaceDirection LookFaceDirection { get; }
        public FaceDirection GridFaceDirection { get; }

        public PlayerModel(IInputAxisController inputAxisController)
        {
            this.inputAxisController = inputAxisController;
            LookFaceDirection = new FaceDirection(new QuadrantDirectionStrategy(), FaceDirectionState.DownAndRight);
            GridFaceDirection = new FaceDirection(new OctagonalDirectionStrategy(), FaceDirectionState.Right);
        }

        public Vector2 UpdateMove(float speed, float deltaTime)
        {
            Vector2 moveVector = new Vector2(inputAxisController.GetHorizontalAxis(), inputAxisController.GetVerticalAxis()) * speed * deltaTime;
            LookFaceDirection.MoveToChangeFaceDirection(moveVector);
            GridFaceDirection.MoveToChangeFaceDirection(moveVector);

            return moveVector;
        }
    }
}