using UnityEngine;

namespace GameCore
{
    public class PlayerModel
    {
        private readonly IInputAxisController inputAxisController;
        private readonly IInGameMapModel inGameMapModel;
        private readonly MovementProcessor movementProcessor;
        private readonly FaceDirection lookFaceDirection;
        private readonly FaceDirection gridFaceDirection;

        public PlayerModel(IInputAxisController inputAxisController, IInGameMapModel inGameMapModel)
        {
            this.inputAxisController = inputAxisController;
            this.inGameMapModel = inGameMapModel;

            lookFaceDirection = new FaceDirection(new QuadrantDirectionStrategy(), FaceDirectionState.DownAndRight);
            gridFaceDirection = new FaceDirection(new OctagonalDirectionStrategy(), FaceDirectionState.Right);
            movementProcessor = new MovementProcessor();
        }

        public InGameMapCell GetCurrentFaceCell(Vector2 pos, Vector2 touchRange)
        {
            InGameMapCell cell = inGameMapModel.GetCellInfo(pos, gridFaceDirection.CurrentFaceDirectionState, touchRange);
            return cell;
        }

        public Vector2 UpdateMove(float speed, float deltaTime)
        {
            Vector2 moveVector = movementProcessor.GetMoveVector(new Vector2(inputAxisController.GetHorizontalAxis(), inputAxisController.GetVerticalAxis()), speed,
                deltaTime);

            lookFaceDirection.MoveToChangeFaceDirection(moveVector);
            gridFaceDirection.MoveToChangeFaceDirection(moveVector);

            return moveVector;
        }

        public void Bind(IPlayerView playerView)
        {
            lookFaceDirection.BindView(playerView);
        }
    }
}