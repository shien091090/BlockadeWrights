using System;
using UnityEngine;
using Zenject;

namespace GameCore
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Transform faceDirRoot;
        [SerializeField] private SpriteRenderer sr_frontSide;
        [SerializeField] private SpriteRenderer sr_backSide;

        [Inject] private PlayerModel playerModel;

        private void Start()
        {
            playerModel.FaceDirection.OnFaceDirectionChanged -= ChangeFaceDirection;
            playerModel.FaceDirection.OnFaceDirectionChanged += ChangeFaceDirection;
        }

        private void Update()
        {
            transform.Translate(playerModel.UpdateMove(moveSpeed, Time.deltaTime));
        }

        private void ChangeFaceDirection(FaceDirectionState faceDirectionState)
        {
            ChangeHorizontalDirection(faceDirectionState);
            ChangeVerticalDirection(faceDirectionState);
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