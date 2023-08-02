using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore
{
    public class GameObjectPoolComponent : MonoBehaviour
    {
        [SerializeField] private int preSpawnCount;
        [SerializeField] private GameObject gameObjectPrefab;
        [SerializeField] private Transform holderRoot;

        private readonly List<GameObject> gameObjectList = new List<GameObject>();

        public void InitPreSpawn()
        {
            for (int i = 0; i < preSpawnCount; i++)
            {
                GameObject go = CreateNewGameObject();
                go.SetActive(false);
            }
        }

        public T SpawnGameObject<T>(Vector2 pos) where T : Object
        {
            GameObject hidingGameObject = gameObjectList.FirstOrDefault(x => x.activeInHierarchy == false);
            GameObject spawnGameObject = hidingGameObject ?? CreateNewGameObject();
            spawnGameObject.transform.position = pos;
            spawnGameObject.SetActive(true);

            return spawnGameObject.GetComponent<T>();
        }

        public void SpawnGameObject(Vector2 pos)
        {
            SpawnGameObject<GameObject>(pos);
        }

        private GameObject CreateNewGameObject()
        {
            GameObject newGameObj = Instantiate(gameObjectPrefab, holderRoot);
            gameObjectList.Add(newGameObj);
            return newGameObj;
        }
    }
}