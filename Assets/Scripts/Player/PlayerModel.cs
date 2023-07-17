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
            FaceDirection = FaceDirection.Down;
        }

        public Vector2 UpdateMove(float speed, float deltaTime)
        {
            return new Vector2(inputAxisController.GetHorizontalAxis(), inputAxisController.GetVerticalAxis()) * speed * deltaTime;
        }
    }
}