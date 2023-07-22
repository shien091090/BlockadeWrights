using UnityEngine;

namespace GameCore
{
    public class InGameMapModel
    {
        private Vector2 CellUnitSize { get; }
        private Vector2 FullMapSize { get; }

        public InGameMapModel(Vector2 mapSize, Vector2 cellSize)
        {
            FullMapSize = mapSize;
            CellUnitSize = cellSize;
        }

        public InGameMapCell GetCellByPosition(Vector3 worldPos)
        {
            if (FullMapSize.x == 0 || FullMapSize.y == 0)
                return InGameMapCell.GetEmptyCell();

            if (CellUnitSize.x == 0 || CellUnitSize.y == 0)
                return InGameMapCell.GetEmptyCell();

            return new InGameMapCell(0, 0);
        }
    }
}