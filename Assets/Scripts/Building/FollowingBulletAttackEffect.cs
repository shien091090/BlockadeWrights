using UnityEngine;

namespace GameCore
{
    public class FollowingBulletAttackEffect : MonoBehaviour, IBuildingAttackView
    {
        [SerializeField] private GameObjectPoolComponent effectObjectPool;

        public void StartAttack(IAttackTarget attackTarget, float attackPower)
        {
            ParticleFollowEffect particleFollowEffect = effectObjectPool.SpawnGameObject<ParticleFollowEffect>(transform.position);
            particleFollowEffect.StartFollow(attackTarget, () =>
            {
                attackTarget.Damage(attackPower);
            });
        }

        private void Start()
        {
            effectObjectPool.InitPreSpawn();
        }
    }
}