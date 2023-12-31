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
        [SerializeField] private GameObject go_gameOverPanel;
        [SerializeField] private GameObject go_questCompletePanel;
        [SerializeField] private RemainMonsterHintView remainMonsterHintView;

        [Inject] private IGameProcessModel gameProcessModel;

        public ITimerView GetTimerView => timerView;
        public IWaveHintView GetWaveHintView => waveHintView;
        public IFortressView GetFortressView => fortressView;
        public IRemainMonsterHintView GetRemainMonsterHintView => remainMonsterHintView;

        public void SetGameOverPanelActive(bool isActive)
        {
            go_gameOverPanel.SetActive(isActive);
        }

        public void SetQuestCompletePanelActive(bool isActive)
        {
            go_questCompletePanel.SetActive(isActive);
        }

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
            gameProcessModel.Bind(this);
            gameProcessModel.StartGame();
        }
    }
}