using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GameCore
{
    public class MonsterHolderView : MonoBehaviour
    {
        [Inject] private MonsterSpawner monsterSpawner;

        [SerializeField] private List<Vector2> pathPointList;

        private GameObjectPoolComponent gameObjectPool;
        private Vector2 StartPoint => pathPointList[0];

        private GameObjectPoolComponent GameObjectPool
        {
            get
            {
                if (gameObjectPool == null)
                    gameObjectPool = GetComponent<GameObjectPoolComponent>();

                return gameObjectPool;
            }
        }

        private void Start()
        {
            SetEventRegister();
            GameObjectPool.InitPreSpawn();
        }

        private MonsterMovementPath GetPathInfo()
        {
            MonsterMovementPath pathInfo = new MonsterMovementPath();
            foreach (Vector2 pos in pathPointList)
            {
                pathInfo.AddPoint(pos);
            }

            return pathInfo;
        }

        private void SetEventRegister()
        {
            monsterSpawner.OnSpawnMonster -= SpawnMonster;
            monsterSpawner.OnSpawnMonster += SpawnMonster;
        }

        [ContextMenu("SpawnMonster")]
        private void SpawnMonster()
        {
            MonsterView monsterView = GameObjectPool.SpawnGameObject<MonsterView>(StartPoint);
            MonsterModel monsterModel = new MonsterModel(GetPathInfo());
            monsterView.Init(monsterModel);
        }
    }
}