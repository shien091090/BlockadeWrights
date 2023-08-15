using TMPro;
using UnityEngine;

namespace GameCore
{
    public class MonsterHolderView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tmp_waveHint;

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


        public void Init(string startWaveHint)
        {
            GameObjectPool.InitPreSpawn();
            SetWaveHint(startWaveHint);
        }


        public void SetWaveHint(string waveHint)
        {
            tmp_waveHint.text = waveHint;
        }

        public void SpawnMonster(IMonsterModel monsterModel)
        {
            MonsterView monsterView = GameObjectPool.SpawnGameObject<MonsterView>(monsterModel.GetStartPoint);
            monsterView.Init(monsterModel);
        }
    }
}