using UnityEngine;

namespace GameCore
{
    public class MonsterView : MonoBehaviour, IAttackTarget
    {
        [SerializeField] private float moveSpeed;

        public Vector2 GetPos => transform.position;
        public string Id => gameObject.GetInstanceID().ToString();
        public bool IsDead => monsterModel.HpModel.IsDead;

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

        private HealthPointComponent HpComponent
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
            throw new System.NotImplementedException();
        }

        private void Update()
        {
            if (monsterModel == null)
                return;

            transform.Translate(monsterModel.UpdateMove(transform.position, moveSpeed, Time.deltaTime));
        }

        public void Init(IMonsterModel monsterModel)
        {
            this.monsterModel = monsterModel;
            HpComponent.Setup(monsterModel.HpModel);
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