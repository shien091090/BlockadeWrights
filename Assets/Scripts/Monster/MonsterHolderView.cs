using TMPro;
using UnityEngine;

namespace GameCore
{
    public class MonsterHolderView : MonoBehaviour
    {
        [SerializeField] private AttackWaveSettingScriptableObject attackWaveSetting;
        [SerializeField] private TextMeshProUGUI tmp_waveHint;

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


        private void Update()
        {
            monsterSpawner.CheckUpdateSpawn(Time.deltaTime);
        }

        private void SetEventRegister()
        {
            monsterSpawner.OnSpawnMonster -= SpawnMonster;
            monsterSpawner.OnSpawnMonster += SpawnMonster;

            monsterSpawner.OnStartNextWave -= RefreshWaveHint;
            monsterSpawner.OnStartNextWave += RefreshWaveHint;
        }

        private void RefreshWaveHint()
        {
            tmp_waveHint.text = monsterSpawner.GetWaveHint;
        }

        private void Awake()
        {
            GameObjectPool.InitPreSpawn();

            monsterSpawner = new MonsterSpawner();
            monsterSpawner.Init(attackWaveSetting.GetAttackWaves());

            SetEventRegister();
            RefreshWaveHint();
        }

        private void SpawnMonster(IMonsterModel monsterModel)
        {
            MonsterView monsterView = GameObjectPool.SpawnGameObject<MonsterView>(monsterModel.GetStartPoint);
            monsterModel.InitHp(attackWaveSetting.MonsterHp);
            monsterView.Init(monsterModel);
        }
    }
}