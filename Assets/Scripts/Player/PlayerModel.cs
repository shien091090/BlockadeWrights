using UnityEngine;

namespace GameCore
{
    public class PlayerModel
    {
        private readonly IInputAxisController inputAxisController;
        private readonly IInGameMapModel inGameMapModel;
        private readonly MovementProcessor movementProcessor;
        public FaceDirection LookFaceDirection { get; }
        public FaceDirection GridFaceDirection { get; }

        public PlayerModel(IInputAxisController inputAxisController, IInGameMapModel inGameMapModel)
        {
            this.inputAxisController = inputAxisController;
            this.inGameMapModel = inGameMapModel;

            LookFaceDirection = new FaceDirection(new QuadrantDirectionStrategy(), FaceDirectionState.DownAndRight);
            GridFaceDirection = new FaceDirection(new OctagonalDirectionStrategy(), FaceDirectionState.Right);
            movementProcessor = new MovementProcessor();
        }

        public Vector2 UpdateMove(float speed, float deltaTime)
        {
            Vector2 moveVector = movementProcessor.GetMoveVector(new Vector2(inputAxisController.GetHorizontalAxis(), inputAxisController.GetVerticalAxis()), speed,
                deltaTime);

            LookFaceDirection.MoveToChangeFaceDirection(moveVector);
            GridFaceDirection.MoveToChangeFaceDirection(moveVector);

            return moveVector;
        }


        public InGameMapCell GetCurrentFaceCell(Vector2 pos, Vector2 touchRange)
        {
            InGameMapCell cell = inGameMapModel.GetCellInfo(pos, GridFaceDirection.CurrentFaceDirectionState, touchRange);
            return cell;
        }
    }
}