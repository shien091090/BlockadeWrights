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
            playerModel.OnFaceDirectionChanged -= ChangeFaceDirection;
            playerModel.OnFaceDirectionChanged += ChangeFaceDirection;
        }

        private void Update()
        {
            transform.Translate(playerModel.UpdateMove(moveSpeed, Time.deltaTime));
        }

        private void ChangeFaceDirection(FaceDirection faceDirection)
        {
            switch (faceDirection)
            {
                case FaceDirection.UpAndRight:
                    sp_backSide.flipX = false;
                    sp_frontSide.gameObject.SetActive(false);
                    sp_backSide.gameObject.SetActive(true);
                    break;

                case FaceDirection.UpAndLeft:
                    sp_backSide.flipX = true;
                    sp_frontSide.gameObject.SetActive(false);
                    sp_backSide.gameObject.SetActive(true);
                    break;

                case FaceDirection.DownAndRight:
                    sp_frontSide.flipX = false;
                    sp_frontSide.gameObject.SetActive(true);
                    sp_backSide.gameObject.SetActive(false);
                    break;

                case FaceDirection.DownAndLeft:
                    sp_frontSide.flipX = true;
                    sp_frontSide.gameObject.SetActive(true);
                    sp_backSide.gameObject.SetActive(false);
                    break;
            }
        }
    }
}