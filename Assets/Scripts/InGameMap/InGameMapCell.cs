using UnityEngine;

namespace GameCore
{
    public struct InGameMapCell : IInGameMapCell
    {
        public InGameMapCell(int gridX, int gridY, Vector2 cellSize, Vector2 fullMapSize)
        {
            GridPosition = new IntVector2(gridX, gridY);
            IsEmpty = gridX < 0 || gridY < 0;
            CellSize = cellSize;
            FullMapSize = fullMapSize;
            CenterPosition = new Vector2(gridX * cellSize.x + cellSize.x / 2 - (fullMapSize.x / 2), gridY * cellSize.y + cellSize.y / 2 - (fullMapSize.y / 2));
        }

        public Vector2 FullMapSize { get; }
        public Vector2 CellSize { get; }
        public bool IsEmpty { get; }
        public IntVector2 GridPosition { get; }
        public Vector2 CenterPosition { get; }

        public static InGameMapCell GetEmptyCell()
        {
            return new InGameMapCell(-1, -1, Vector2.zero, Vector2.zero);
        }
    }
}