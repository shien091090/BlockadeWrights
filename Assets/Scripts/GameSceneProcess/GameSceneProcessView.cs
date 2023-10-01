using UnityEngine;
using Zenject;

namespace GameCore
{
    public class GameSceneProcessView : MonoBehaviour
    {
        [SerializeField] private WaveHintView waveHintView;
        [SerializeField] private FortressView fortressView;
        [SerializeField] private float fortressHp;
        [SerializeField] private GameObjectPoolComponent monsterObjectPool;
        [SerializeField] private TimerView timerView;

        [Inject] private IMonsterSpawner monsterSpawner;

        private FortressModel fortressModel;

        private void Update()
        {
            monsterSpawner.CheckUpdateSpawn(Time.deltaTime);
        }

        public void Start()
        {
            waveHintView.SetWaveHint(monsterSpawner.GetWaveHint);

            fortressModel = new FortressModel(fortressHp, monsterSpawner);
            fortressView.Init(fortressModel);

            monsterObjectPool.InitPreSpawn();

            SetEventRegister();
            CheckStartTimer();
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

        private void CheckStartTimer()
        {
            if (monsterSpawner.IsNeedCountDownToSpawnMonster())
                timerView.StartCountDown(monsterSpawner.GetStartTimeSeconds());
        }

        private void OnFortressDestroy()
        {
            fortressView.SetDestroyHintActive(true);
        }

        private void OnStartNextWave()
        {
            waveHintView.SetWaveHint(monsterSpawner.GetWaveHint);
        }

        private void OnSpawnMonster(IMonsterModel monsterModel)
        {
            MonsterView monsterView = monsterObjectPool.SpawnGameObject<MonsterView>(monsterModel.GetStartPoint);
            monsterView.Init(monsterModel);
        }
    }
}