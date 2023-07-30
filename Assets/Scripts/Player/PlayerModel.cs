using UnityEngine;

namespace GameCore
{
    public class PlayerModel
    {
        private readonly IInputAxisController inputAxisController;
        private readonly IInputKeyController inputKeyController;
        private readonly IInGameMapModel inGameMapModel;
        private readonly IPlayerOperationModel playerOperationModel;
        public FaceDirection LookFaceDirection { get; }
        public FaceDirection GridFaceDirection { get; }

        public PlayerModel(IInputAxisController inputAxisController, IInputKeyController inputKeyController, IInGameMapModel inGameMapModel,
            IPlayerOperationModel playerOperationModel)
        {
            this.inputAxisController = inputAxisController;
            this.inGameMapModel = inGameMapModel;
            this.playerOperationModel = playerOperationModel;
            this.inputKeyController = inputKeyController;

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

        public void UpdateCheckBuild(IInGameMapCell cell)
        {
            if (inputKeyController.GetBuildKeyDown())
                playerOperationModel.CreateBuilding(cell);
        }

        public InGameMapCell GetCurrentFaceCell(Vector2 pos, Vector2 touchRange)
        {
            InGameMapCell cell = inGameMapModel.GetCellInfo(pos, GridFaceDirection.CurrentFaceDirectionState, touchRange);
            return cell;
        }
    }
}