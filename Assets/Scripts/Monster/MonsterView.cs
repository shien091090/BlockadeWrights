using UnityEngine;

namespace GameCore
{
    public class MonsterView : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;

        private HealthPointComponent hpComponent;
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

        private HealthPointComponent HpComponent
        {
            get
            {
                if (hpComponent == null)
                    hpComponent = GetComponent<HealthPointComponent>();

                return hpComponent;
            }
        }

        private void Update()
        {
            if (monsterModel == null)
                return;

            transform.Translate(monsterModel.UpdateMove(transform.position, moveSpeed, Time.deltaTime));
        }

        public void Init(MonsterModel monsterModel)
        {
            this.monsterModel = monsterModel;
            HpComponent.Setup(monsterModel.HealthPointModel);
            RegisterEvent();
        }

        private void RegisterEvent()
        {
            monsterModel.LookFaceDirection.OnFaceDirectionChanged -= FaceDirection.RefreshFaceDirection;
            monsterModel.LookFaceDirection.OnFaceDirectionChanged += FaceDirection.RefreshFaceDirection;

            monsterModel.OnDead -= OnDead;
            monsterModel.OnDead += OnDead;
        }

        private void OnDead()
        {
            gameObject.SetActive(false);
        }
    }
}