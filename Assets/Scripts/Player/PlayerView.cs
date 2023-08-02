using UnityEngine;
using Zenject;

namespace GameCore
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Vector2 touchRange;
        [SerializeField] private Transform cellHint;

        [Inject] private PlayerModel playerModel;
        [Inject] private IPlayerOperationModel playerOperationModel;

        private FaceDirectionComponent faceDirection;

        private FaceDirectionComponent FaceDirection
        {
            get
            {
                if (faceDirection == null)
                    faceDirection = GetComponent<FaceDirectionComponent>();

                return faceDirection;
            }
        }

        private void Start()
        {
            RegisterEvent();
        }

        private void Update()
        {
            transform.Translate(playerModel.UpdateMove(moveSpeed, Time.deltaTime));

            InGameMapCell cell = playerModel.GetCurrentFaceCell(transform.position, touchRange);
            playerOperationModel.UpdateCheckBuild(cell);
            RefreshCellHint(cell);
        }

        private void RefreshCellHint(InGameMapCell cell)
        {
            cellHint.gameObject.SetActive(cell.IsEmpty == false);

            if (cell.IsEmpty == false)
                cellHint.position = cell.CenterPosition;
        }

        private void RegisterEvent()
        {
            playerModel.LookFaceDirection.OnFaceDirectionChanged -= FaceDirection.RefreshFaceDirection;
            playerModel.LookFaceDirection.OnFaceDirectionChanged += FaceDirection.RefreshFaceDirection;
        }
    }
}