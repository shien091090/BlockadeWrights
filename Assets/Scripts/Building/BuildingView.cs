using UnityEngine;

namespace GameCore
{
    public class BuildingView : MonoBehaviour
    {
        [SerializeField] private CircleMeshEffect circleMeshEffect;
        [SerializeField] private int attackRange;
        [SerializeField] private float attackFrequency;
        [SerializeField] private float attackPower;
        [SerializeField] private TriggerColliderComponent triggerCollider;

        private AutoAttackModel autoAttackModel;

        private void Start()
        {
            autoAttackModel = new AutoAttackModel(attackRange, attackFrequency, transform.position, attackPower);
            circleMeshEffect.ShowEffect(attackRange);
            triggerCollider.InitHandler(autoAttackModel);
        }

        private void Update()
        {
            autoAttackModel.UpdateAttackTimer(Time.deltaTime);
        }
    }
}