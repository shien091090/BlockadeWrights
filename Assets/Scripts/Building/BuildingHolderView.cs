using UnityEngine;
using Zenject;

namespace GameCore
{
    public class BuildingHolderView : MonoBehaviour
    {
        [Inject] private IPlayerOperationModel playerOperationModel;
        private GameObjectPoolComponent gameObjectPoolComponent;

        private GameObjectPoolComponent GameObjectPoolComponent
        {
            get
            {
                if (gameObjectPoolComponent == null)
                    gameObjectPoolComponent = GetComponent<GameObjectPoolComponent>();

                return gameObjectPoolComponent;
            }
        }

        private void Start()
        {
            RegisterEvent();
            GameObjectPoolComponent.InitPreSpawn();
        }

        private void RegisterEvent()
        {
            playerOperationModel.OnCreateBuilding -= OnBuildNewBuilding;
            playerOperationModel.OnCreateBuilding += OnBuildNewBuilding;
        }

        private void OnBuildNewBuilding(IInGameMapCell targetMapCell)
        {
            GameObjectPoolComponent.SpawnEntity(targetMapCell.CenterPosition);
        }
    }
}