using UnityEngine;

namespace GameCore
{
    public class MonsterHolderView : MonoBehaviour
    {
        [SerializeField] private AttackWaveSettingScriptableObject attackWaveSetting;

        private MonsterSpawner monsterSpawner;
        private GameObjectPoolComponent gameObjectPool;

        private GameObjectPoolComponent GameObjectPool
        {
            get
            {
                if (gameObjectPool == null)
                    gameObjectPool = GetComponent<GameObjectPoolComponent>();

                return gameObjectPool;
            }
        }

        private void Awake()
        {
            GameObjectPool.InitPreSpawn();

            monsterSpawner = new MonsterSpawner();
            monsterSpawner.Init(attackWaveSetting.GetAttackWaves);
            
            SetEventRegister();
        }

        private void Update()
        {
            monsterSpawner.CheckUpdateSpawn(Time.deltaTime);
        }

        private void SetEventRegister()
        {
            monsterSpawner.OnSpawnMonster -= SpawnMonster;
            monsterSpawner.OnSpawnMonster += SpawnMonster;
        }

        [ContextMenu("SpawnMonster")]
        private void SpawnMonster()
        {
            MonsterView monsterView = GameObjectPool.SpawnGameObject<MonsterView>(attackWaveSetting.StartPoint);
            MonsterModel monsterModel = new MonsterModel(attackWaveSetting.GetPathInfo());
            monsterModel.InitHp(100);
            monsterView.Init(monsterModel);
        }
    }
}