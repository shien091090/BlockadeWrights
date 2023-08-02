using UnityEngine;

namespace GameCore
{
    public class MonsterView : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;

        public bool IsActive => gameObject.activeInHierarchy;
        private MonsterModel monsterModel;

        private void Update()
        {
            if (monsterModel == null)
                return;

            transform.Translate(monsterModel.UpdateMove(transform.position, moveSpeed, Time.deltaTime));
        }

        public void Init(MonsterModel monsterModel)
        {
            this.monsterModel = monsterModel;
        }
    }
}