using UnityEngine;
using Zenject;

namespace GameCore
{
    public class BuildingHolderView : MonoBehaviour
    {
        [Inject] private IPlayerOperationModel playerOperationModel;
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

        private void Start()
        {
            RegisterEvent();
            GameObjectPool.InitPreSpawn();
        }

        private void RegisterEvent()
        {
            playerOperationModel.OnCreateBuilding -= OnBuildNewBuilding;
            playerOperationModel.OnCreateBuilding += OnBuildNewBuilding;
        }

        private void OnBuildNewBuilding(IInGameMapCell targetMapCell)
        {
            GameObjectPool.SpawnEntity(targetMapCell.CenterPosition);
        }
    }
}