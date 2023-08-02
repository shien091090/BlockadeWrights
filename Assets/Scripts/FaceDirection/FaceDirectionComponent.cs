using UnityEngine;

namespace GameCore
{
    public class FaceDirectionComponent : MonoBehaviour
    {
        [SerializeField] private GameObject go_frontSide;
        [SerializeField] private GameObject go_backSide;
        [SerializeField] private Transform faceDirRoot;

        public void RefreshFaceDirection(FaceDirectionState faceDirectionState)
        {
            ChangeHorizontalDirection(faceDirectionState);
            ChangeVerticalDirection(faceDirectionState);
        }

        private void ChangeVerticalDirection(FaceDirectionState faceDirectionState)
        {
            go_frontSide.SetActive(faceDirectionState == FaceDirectionState.DownAndLeft || faceDirectionState == FaceDirectionState.DownAndRight);
            go_backSide.SetActive(faceDirectionState == FaceDirectionState.UpAndLeft || faceDirectionState == FaceDirectionState.UpAndRight);
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