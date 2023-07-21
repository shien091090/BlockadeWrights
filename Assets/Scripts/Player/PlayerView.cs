using System;
using UnityEngine;
using Zenject;

namespace GameCore
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private SpriteRenderer sp_frontSide;
        [SerializeField] private SpriteRenderer sp_backSide;

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
            switch (faceDirectionState)
            {
                case FaceDirectionState.UpAndRight:
                    sp_backSide.flipX = false;
                    sp_frontSide.gameObject.SetActive(false);
                    sp_backSide.gameObject.SetActive(true);
                    break;

                case FaceDirectionState.UpAndLeft:
                    sp_backSide.flipX = true;
                    sp_frontSide.gameObject.SetActive(false);
                    sp_backSide.gameObject.SetActive(true);
                    break;

                case FaceDirectionState.DownAndRight:
                    sp_frontSide.flipX = false;
                    sp_frontSide.gameObject.SetActive(true);
                    sp_backSide.gameObject.SetActive(false);
                    break;

                case FaceDirectionState.DownAndLeft:
                    sp_frontSide.flipX = true;
                    sp_frontSide.gameObject.SetActive(true);
                    sp_backSide.gameObject.SetActive(false);
                    break;
            }
        }
    }
}