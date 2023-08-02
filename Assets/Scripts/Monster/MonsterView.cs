using UnityEngine;

namespace GameCore
{
    public class MonsterView : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;

        private MonsterModel monsterModel;
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

        private void RegisterEvent()
        {
            monsterModel.LookFaceDirection.OnFaceDirectionChanged -= FaceDirection.RefreshFaceDirection;
            monsterModel.LookFaceDirection.OnFaceDirectionChanged += FaceDirection.RefreshFaceDirection;
        }
    }
}