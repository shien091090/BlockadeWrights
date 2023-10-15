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

        public ITimerView GetTimerView => timerView;
        public IWaveHintView GetWaveHintView => waveHintView;
        public IFortressView GetFortressView => fortressView;
        private GameProcessModel gameProcessModel;

        public IMonsterView SpawnMonsterView(IMonsterModel monsterModel)
        {
            MonsterView monster = monsterObjectPool.SpawnGameObject<MonsterView>(monsterModel.GetStartPoint);
            return monster;
        }

        private void Update()
        {
            gameProcessModel.Update();
        }

        public void Start()
        {
            monsterObjectPool.InitPreSpawn();
        }
    }
}