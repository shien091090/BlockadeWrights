using UnityEngine;

namespace GameCore
{
    public class MonsterView : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private GameObject go_frontSide;
        [SerializeField] private GameObject go_backSide;
        [SerializeField] private Transform faceDirRoot;

        private MonsterModel monsterModel;

        public bool IsActive => gameObject.activeInHierarchy;

        private void Update()
        {
            if (monsterModel == null)
                return;

            transform.Translate(monsterModel.UpdateMove(transform.position, moveSpeed, Time.deltaTime));
        }

        private void Start()
        {
            RegisterEvent();
        }

        public void Init(MonsterModel monsterModel)
        {
            this.monsterModel = monsterModel;
        }

        private void RefreshFaceDirection(FaceDirectionState faceDirectionState)
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

        private void RegisterEvent()
        {
            monsterModel.LookFaceDirection.OnFaceDirectionChanged -= RefreshFaceDirection;
            monsterModel.LookFaceDirection.OnFaceDirectionChanged += RefreshFaceDirection;
        }
    }
}