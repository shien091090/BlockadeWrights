using UnityEngine;

namespace GameCore
{
    public class MonsterView : MonoBehaviour, IAttackTarget
    {
        [SerializeField] private SpriteRenderer sp_frontSide;
        [SerializeField] private SpriteRenderer sp_backSide;

        public ITransform GetTransform
        {
            get
            {
                if (transformAdapter == null)
                    transformAdapter = GetComponent<TransformComponent>();

                return transformAdapter;
            }
        }

        public string Id => gameObject.GetInstanceID().ToString();
        public bool IsDead => monsterModel.HpModel.IsDead;

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

        public void Damage(float damageValue)
        {
            monsterModel.HpModel.Damage(damageValue);
        }

        private void Update()
        {
            if (monsterModel == null)
                return;

            transform.Translate(monsterModel.UpdateMove(transform.position, monsterModel.MoveSpeed, Time.deltaTime));
        }

        public void Init(IMonsterModel monsterModel)
        {
            this.monsterModel = monsterModel;
            GetHpComponent.Setup(monsterModel.HpModel);
            sp_frontSide.sprite = monsterModel.GetFrontSideSprite;
            sp_backSide.sprite = monsterModel.GetBackSideSprite;
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