using System;
using UnityEngine;
using Zenject;

namespace GameCore
{
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Vector2 touchRange;
        [SerializeField] private Transform cellHint;

        [Inject] private PlayerModel playerModel;
        [Inject] private IPlayerOperationModel playerOperationModel;

        private FaceDirectionComponent faceDirection;

        public void RefreshFaceDirection(FaceDirectionState faceDirectionState)
        {
            faceDirection.RefreshFaceDirection(faceDirectionState);
        }

        private void Start()
        {
            playerModel.Bind(this);
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

        private void Awake()
        {
            faceDirection = GetComponent<FaceDirectionComponent>();
        }
    }
}