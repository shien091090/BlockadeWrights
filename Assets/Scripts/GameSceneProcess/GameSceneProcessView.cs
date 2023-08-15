using UnityEngine;

namespace GameCore
{
    public class GameSceneProcessView : MonoBehaviour
    {
        [SerializeField] private AttackWaveSettingScriptableObject attackWaveSetting;
        [SerializeField] private MonsterHolderView monsterHolderView;
        [SerializeField] private FortressView fortressView;
        [SerializeField] private float fortressHp;

        private MonsterSpawner monsterSpawner;
        private FortressModel fortressModel;

        private void Update()
        {
            monsterSpawner.CheckUpdateSpawn(Time.deltaTime);
        }

        public void Awake()
        {
            monsterSpawner = new MonsterSpawner();
            monsterSpawner.Init(attackWaveSetting.GetAttackWaves());

            monsterHolderView.Init(monsterSpawner.GetWaveHint);

            fortressModel = new FortressModel(fortressHp, monsterSpawner);
            fortressView.Init(fortressModel);

            SetEventRegister();
        }

        private void SetEventRegister()
        {
            monsterSpawner.OnSpawnMonster -= OnSpawnMonster;
            monsterSpawner.OnSpawnMonster += OnSpawnMonster;

            monsterSpawner.OnStartNextWave -= OnStartNextWave;
            monsterSpawner.OnStartNextWave += OnStartNextWave;

            fortressModel.OnFortressDestroy -= OnFortressDestroy;
            fortressModel.OnFortressDestroy += OnFortressDestroy;
        }

        private void OnFortressDestroy()
        {
            fortressView.SetDestroyHintActive(true);
        }

        private void OnStartNextWave()
        {
            monsterHolderView.SetWaveHint(monsterSpawner.GetWaveHint);
        }

        private void OnSpawnMonster(IMonsterModel monsterModel)
        {
            monsterModel.InitHp(attackWaveSetting.MonsterHp);
            monsterHolderView.SpawnMonster(monsterModel);
        }
    }
}