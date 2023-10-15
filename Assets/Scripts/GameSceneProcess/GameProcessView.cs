using UnityEngine;
using Zenject;

namespace GameCore
{
    public class GameProcessView : MonoBehaviour, IGameProcessView
    {
        [SerializeField] private WaveHintView waveHintView;
        [SerializeField] private FortressView fortressView;
        [SerializeField] private GameObjectPoolComponent monsterObjectPool;
        [SerializeField] private TimerView timerView;

        [Inject] private IMonsterSpawner monsterSpawner;

        public ITimerView GetTimerView => timerView;
        public IWaveHintView GetWaveHintView => waveHintView;
        public IFortressView GetFortressView => fortressView;

        public IMonsterView SpawnMonsterView(IMonsterModel monsterModel)
        {
            MonsterView monster = monsterObjectPool.SpawnGameObject<MonsterView>(monsterModel.GetStartPoint);
            return monster;
        }

        private void Update()
        {
            monsterSpawner.CheckUpdateSpawn(Time.deltaTime);
        }

        public void Start()
        {
            monsterObjectPool.InitPreSpawn();
        }
    }
}