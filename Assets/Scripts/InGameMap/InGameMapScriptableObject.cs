using UnityEngine;

namespace GameCore
{
    public class InGameMapScriptableObject : ScriptableObject, IInGameMapSetting
    {
        [SerializeField] private Vector2 mapSize;
        [SerializeField] private Vector2 cellSize;

        public Vector2 MapSize => mapSize;
        public Vector2 CellSize => cellSize;
    }
}