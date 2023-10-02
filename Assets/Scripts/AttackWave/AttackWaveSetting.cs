using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore
{
    [System.Serializable]
    public class AttackWaveSetting
    {
        [SerializeField] private float startTimeSecond;
        [SerializeField] private float spawnIntervalSecond;
        [SerializeField] private List<Vector2> pathPointList;
        [SerializeField] private List<MonsterOrderSetting> monsterSpawnOrderSetting;

        private bool isPathEditorMode;
        private List<EditorPathHint> pathHintObjectList;
        private GameObject pathHintRoot;

        public float StartTimeSecond => startTimeSecond;
        public float SpawnIntervalSecond => spawnIntervalSecond;
        public List<Vector2> PathPointList => pathPointList;

        public List<IMonsterSetting> GetMonsterOrderList()
        {
            List<IMonsterSetting> orderList = new List<IMonsterSetting>();
            foreach (MonsterOrderSetting orderSetting in monsterSpawnOrderSetting)
            {
                for (int i = 0; i < orderSetting.SpawnCount; i++)
                {
                    orderList.Add(orderSetting.MonsterSetting);
                }
            }

            return orderList;
        }

        private void SetPathEditorMode(bool isOn)
        {
            ResetPathHint();

            if (isOn)
                CreatePathHints();
        }

        private void RefreshDrawPathHint()
        {
            foreach (EditorPathHint pathHintObject in pathHintObjectList)
            {
                pathHintObject.RefreshDrawPathHint();
            }
        }

        private void ResetPathHint()
        {
            pathHintObjectList = new List<EditorPathHint>();
            GameObject.DestroyImmediate(pathHintRoot);
            pathHintRoot = null;
        }

        private void CreatePathHints()
        {
            pathHintRoot = new GameObject();
            pathHintRoot.name = "[PathHintRoot]";

            for (int index = 0; index < pathPointList.Count; index++)
            {
                Vector2 pos = pathPointList[index];
                GameObject pathHintObj = new GameObject();
                pathHintObj.name = $"PathHint_{index}";
                pathHintObj.transform.position = pos;
                pathHintObj.transform.SetParent(pathHintRoot.transform);
                pathHintObj.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("monster_path_hint");
                LineRenderer lineRenderer = pathHintObj.AddComponent<LineRenderer>();
                if (index < pathPointList.Count - 1)
                {
                    lineRenderer.positionCount = 2;
                    lineRenderer.startWidth = 0.05f;
                    lineRenderer.endWidth = 0.05f;
                }

                EditorPathHint pathHint = new EditorPathHint(pathHintObj.transform, lineRenderer);
                pathHintObjectList.Add(pathHint);
            }

            for (int index = 0; index < pathHintObjectList.Count - 1; index++)
            {
                EditorPathHint pathHintObject = pathHintObjectList[index];
                pathHintObject.SetNextPathHint(pathHintObjectList[index + 1].Transform);
            }

            RefreshDrawPathHint();
        }

        [ShowIf("isPathEditorMode")]
        [VerticalGroup("split/left")]
        [Button("PathEditorMode : On"), GUIColor(0.3f, 0.9f, 0.3f)]
        private void OpenPathEditorModeButton()
        {
            isPathEditorMode = !isPathEditorMode;
            SetPathEditorMode(isPathEditorMode);
        }

        [HideIf("isPathEditorMode")]
        [VerticalGroup("split/left")]
        [Button("PathEditorMode : Off"), GUIColor(0.9f, 0.3f, 0.3f)]
        private void ClosePathEditorModeButton()
        {
            isPathEditorMode = !isPathEditorMode;
            SetPathEditorMode(isPathEditorMode);
        }
        
        [HorizontalGroup("split", 0.5f)]
        [Button("RefreshDrawPathHint")]
        private void RefreshDrawPathHintButton()
        {
            RefreshDrawPathHint();
        }
    }
}