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
        public IHealthPointView GetHealthPointView => healthPointView;

        private ITransform transformAdapter;
        private IMonsterModel monsterModel;
        private FaceDirectionComponent faceDirection;
        private IHealthPointView healthPointView;

        public void InitSprite(Sprite frontSide, Sprite backSide)
        {
            sp_frontSide.sprite = frontSide;
            sp_backSide.sprite = backSide;
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
            healthPointView = GetComponent<HealthPointComponent>();
            faceDirection = GetComponent<FaceDirectionComponent>();
        }
    }
}