using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore
{
    public class BuildingHolderView : MonoBehaviour
    {
        private const int PRE_SPAWN_COUNT = 8;

        [SerializeField] private BuildingView buildingPrefab;
        [SerializeField] private Transform holderRoot;
        
        private readonly List<BuildingView> buildingList = new List<BuildingView>();

        private void Start()
        {
            InitBuildingPool();
        }

        private void InitBuildingPool()
        {
            for (int i = 0; i < PRE_SPAWN_COUNT; i++)
            {
                BuildingView newEmptyBuilding = CreateNewBuilding();
                newEmptyBuilding.gameObject.SetActive(false);
            }
        }

        public void CreateBuilding(Vector2 pos)
        {
            BuildingView hidingBuilding = buildingList.FirstOrDefault(x => x.gameObject.activeSelf == false);
            BuildingView buildingView = hidingBuilding;
            if (buildingView == null)
                buildingView = CreateNewBuilding();

            buildingView.transform.position = pos;
        }

        private BuildingView CreateNewBuilding()
        {
            BuildingView newBuilding = Instantiate(buildingPrefab, holderRoot);
            buildingList.Add(newBuilding);
            return newBuilding;
        }
    }
}