using UnityEngine;

namespace GameCore
{
    public class BuildingView : MonoBehaviour
    {
        [SerializeField] private CircleMeshEffect circleMeshEffect;
        [SerializeField] private int attackRange;
        [SerializeField] private float attackFrequency;
        [SerializeField] private float attackPower;

        private AutoAttackModel autoAttackModel;

        private void Start()
        {
            autoAttackModel = new AutoAttackModel(attackRange, attackFrequency, transform.position, attackPower);
            circleMeshEffect.ShowEffect(attackRange);
        }

        private void Update()
        {
            autoAttackModel.UpdateAttackTimer(Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(GameConst.GAME_OBJECT_TAG_MONSTER))
            {
                Debug.Log("BuildingView OnTriggerEnter");
            }
        }
    }
}