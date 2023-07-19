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
            Vector2 currentFaceVector = PlayerFaceDir switch
            {
                FaceDirection.UpAndRight => new Vector2(1, 1),
                FaceDirection.UpAndLeft => new Vector2(-1, 1),
                FaceDirection.DownAndRight => new Vector2(1, -1),
                FaceDirection.DownAndLeft => new Vector2(-1, -1),
                _ => Vector2.zero
            };

            float xAxisReverse = 1;
            if(( moveVector.x > 0 && currentFaceVector.x < 0 ) || ( moveVector.x < 0 && currentFaceVector.x > 0 ))
                xAxisReverse = -1;
            
            float yAxisReverse = 1;
            if((moveVector.y > 0 && currentFaceVector.y < 0) || (moveVector.y < 0 && currentFaceVector.y > 0))
                yAxisReverse = -1;

            Vector2 reverseVector = new Vector2(xAxisReverse, yAxisReverse);

            currentFaceVector *= reverseVector;
            if(currentFaceVector.x == 1 && currentFaceVector.y == 1)
                return FaceDirection.UpAndRight;
            else if(currentFaceVector.x == 1 && currentFaceVector.y == -1)
                return FaceDirection.DownAndRight;
            else if(currentFaceVector.x == -1 && currentFaceVector.y == 1)
                return FaceDirection.UpAndLeft;
            else if(currentFaceVector.x == -1 && currentFaceVector.y == -1)
                return FaceDirection.DownAndLeft;
            else
                return FaceDirection.DownAndRight;
            
        }
    }
}