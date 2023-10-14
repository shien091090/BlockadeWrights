using System;
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
        private IMonsterModel monsterModel;
        private HealthPointComponent hpComponent;
        private FaceDirectionComponent faceDirection;

        public void InitSprite(Sprite frontSide, Sprite backSide)
        {
            sp_frontSide.sprite = frontSide;
            sp_backSide.sprite = backSide;
        }

        public void SetupHp(HealthPointModel monsterModelHpModel)
        {
            hpComponent.BindModel(monsterModelHpModel);
        }

        public void RefreshFaceDirection(FaceDirectionState faceDirectionState)
        {
            faceDirection.RefreshFaceDirection(faceDirectionState);
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

        private void Awake()
        {
            hpComponent = GetComponent<HealthPointComponent>();
            faceDirection = GetComponent<FaceDirectionComponent>();
        }
    }
}