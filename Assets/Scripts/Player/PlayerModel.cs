using UnityEngine;

namespace GameCore
{
    public class PlayerModel
    {
        private readonly IInputAxisController inputAxisController;
        public FaceDirection FaceDirection { get; }

        public PlayerModel(IInputAxisController inputAxisController)
        {
            this.inputAxisController = inputAxisController;
            FaceDirection = new FaceDirection(new QuadrantDirectionStrategy(), FaceDirectionState.DownAndRight);
        }

        public Vector2 UpdateMove(float speed, float deltaTime)
        {
            Vector2 moveVector = new Vector2(inputAxisController.GetHorizontalAxis(), inputAxisController.GetVerticalAxis()) * speed * deltaTime;
            FaceDirection.MoveToChangeFaceDirection(moveVector);

            return moveVector;
        }
    }
}