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
        private readonly IPlayerOperationModel playerOperationModel;
        private readonly ITimeManager timeAdapter;
        private IPlayerView playerView;
        private float moveSpeed;


        public PlayerModel(IInputAxisController inputAxisController, IInGameMapModel inGameMapModel, IPlayerOperationModel playerOperationModel, ITimeManager timeAdapter)
        {
            this.inputAxisController = inputAxisController;
            this.inGameMapModel = inGameMapModel;
            this.playerOperationModel = playerOperationModel;
            this.timeAdapter = timeAdapter;

            lookFaceDirection = new FaceDirection(new QuadrantDirectionStrategy(), FaceDirectionState.DownAndRight);
            gridFaceDirection = new FaceDirection(new OctagonalDirectionStrategy(), FaceDirectionState.Right);
            movementProcessor = new MovementProcessor();
        }

        public void Update()
        {
            Vector2 translationVector = UpdateMove(moveSpeed, timeAdapter.DeltaTime);
            if (translationVector != default)
                playerView.GetTransform.Translate(translationVector);

            InGameMapCell cell = GetCurrentFaceCell(playerView.GetTransform.Position, playerView.TouchRange);
            playerOperationModel.UpdateCheckBuild(cell);
            playerView.SetCellHintActive(cell.IsEmpty == false);

            if (cell.IsEmpty == false)
                playerView.SetCellHintPosition(cell.CenterPosition);
        }

        public void Bind(IPlayerView playerView)
        {
            this.playerView = playerView;
            moveSpeed = playerView.MoveSpeed;
            lookFaceDirection.BindView(playerView);
        }

        private InGameMapCell GetCurrentFaceCell(Vector2 pos, Vector2 touchRange)
        {
            InGameMapCell cell = inGameMapModel.GetCellInfo(pos, gridFaceDirection.CurrentFaceDirectionState, touchRange);
            return cell;
        }

        private Vector2 UpdateMove(float speed, float deltaTime)
        {
            Vector2 moveVector = movementProcessor.GetMoveVector(new Vector2(inputAxisController.GetHorizontalAxis(), inputAxisController.GetVerticalAxis()), speed,
                deltaTime);

            lookFaceDirection.MoveToChangeFaceDirection(moveVector);
            gridFaceDirection.MoveToChangeFaceDirection(moveVector);

            return moveVector;
        }
    }
}