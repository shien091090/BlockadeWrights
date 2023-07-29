using System;
using GameCore.Tests.Player;
using UnityEngine;
using Zenject;

namespace GameCore
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Vector2 touchRange;
        [SerializeField] private Transform faceDirRoot;
        [SerializeField] private SpriteRenderer sr_frontSide;
        [SerializeField] private SpriteRenderer sr_backSide;
        [SerializeField] private Transform cellHint;

        [Inject] private PlayerModel playerModel;

        private void Start()
        {
            RegisterEvent();
        }

        private void Update()
        {
            transform.Translate(playerModel.UpdateMove(moveSpeed, Time.deltaTime));

            InGameMapCell cell = playerModel.GetCurrentFaceCell(transform.position, touchRange);
            if (playerModel.UpdateCheckClickBuildButton())
                playerModel.BuildBuilding(cell);

            RefreshCellHintPos(cell);
        }

        private void RefreshCellHintPos(InGameMapCell cell)
        {
            cellHint.gameObject.SetActive(cell.IsEmpty == false);

            if (cell.IsEmpty == false)
                cellHint.position = cell.CenterPosition;
        }

        private void RefreshFaceDirection(FaceDirectionState faceDirectionState)
        {
            ChangeHorizontalDirection(faceDirectionState);
            ChangeVerticalDirection(faceDirectionState);
        }

        private void RegisterEvent()
        {
            playerModel.LookFaceDirection.OnFaceDirectionChanged -= RefreshFaceDirection;
            playerModel.LookFaceDirection.OnFaceDirectionChanged += RefreshFaceDirection;
        }

        private void ChangeVerticalDirection(FaceDirectionState faceDirectionState)
        {
            sr_frontSide.gameObject.SetActive(faceDirectionState == FaceDirectionState.DownAndLeft || faceDirectionState == FaceDirectionState.DownAndRight);
            sr_backSide.gameObject.SetActive(faceDirectionState == FaceDirectionState.UpAndLeft || faceDirectionState == FaceDirectionState.UpAndRight);
        }

        private void ChangeHorizontalDirection(FaceDirectionState faceDirectionState)
        {
            switch (faceDirectionState)
            {
                case FaceDirectionState.UpAndRight:
                case FaceDirectionState.DownAndRight:
                    faceDirRoot.localScale = new Vector3(1, 1, 1);
                    break;

                case FaceDirectionState.UpAndLeft:
                case FaceDirectionState.DownAndLeft:
                    faceDirRoot.localScale = new Vector3(-1, 1, 1);
                    break;
            }
        }
    }
}