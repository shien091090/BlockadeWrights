using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore
{
    public class GameObjectPoolComponent : MonoBehaviour
    {
        [SerializeField] private int preSpawnCount;
        [SerializeField] private BuildingView entityPrefab;
        [SerializeField] private Transform holderRoot;

        private readonly List<IGameObjectPoolEntity> gameObjectList = new List<IGameObjectPoolEntity>();

        public void InitPreSpawn()
        {
            for (int i = 0; i < preSpawnCount; i++)
            {
                IGameObjectPoolEntity newEntity = CreateNewEntity();
                newEntity.Hide();
            }
        }

        public void SpawnEntity(Vector2 pos)
        {
            IGameObjectPoolEntity hidingEntity = gameObjectList.FirstOrDefault(x => x.IsActive == false);
            IGameObjectPoolEntity entity = hidingEntity ?? CreateNewEntity();
            entity.SetPos(pos);
            entity.Show();
        }

        private IGameObjectPoolEntity CreateNewEntity()
        {
            IGameObjectPoolEntity newEntity = Instantiate(entityPrefab, holderRoot);
            gameObjectList.Add(newEntity);
            return newEntity;
        }
    }
}