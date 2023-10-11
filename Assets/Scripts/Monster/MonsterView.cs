using UnityEngine;

namespace GameCore
{
    public class MonsterView : MonoBehaviour, IAttackTargetProvider, IMonsterView
    {
        [SerializeField] private SpriteRenderer sp_frontSide;
        [SerializeField] private SpriteRenderer sp_backSide;

        public IAttackTarget GetAttackTarget => monsterModel;

        public ITransform GetTransform
        {
            get
            {
                if (transformAdapter == null)
                    transformAdapter = GetComponent<TransformComponent>();

                return transformAdapter;
            }
        }

        public string GetId => gameObject.GetInstanceID().ToString();

        private ITransform transformAdapter;
        private HealthPointComponent hpComponent;
        private IMonsterModel monsterModel;
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

        private HealthPointComponent GetHpComponent
        {
            get
            {
                if (hpComponent == null)
                    hpComponent = GetComponent<HealthPointComponent>();

                return hpComponent;
            }
        }

        public void InitSprite(Sprite frontSide, Sprite backSide)
        {
            sp_frontSide.sprite = frontSide;
            sp_backSide.sprite = backSide;
        }

        public void SetupHp(HealthPointModel monsterModelHpModel)
        {
            GetHpComponent.Setup(monsterModelHpModel);
        }

        public void RefreshFaceDirection(FaceDirectionState faceDirectionState)
        {
            FaceDirection.RefreshFaceDirection(faceDirectionState);
        }

        public void Bind(MonsterModel model)
        {
            monsterModel = model;
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void Update()
        {
            monsterModel?.Update();
        }
    }
}