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

        public InGameMapCell GetCellInfo(Vector3 pos)
        {
            if (FullMapSize.x == 0 || FullMapSize.y == 0)
                return InGameMapCell.GetEmptyCell();

            if (CellUnitSize.x == 0 || CellUnitSize.y == 0)
                return InGameMapCell.GetEmptyCell();

            if(pos.x >= FullMapSize.x / 2 || pos.y >= FullMapSize.y / 2 || pos.x < -FullMapSize.x / 2 || pos.y < -FullMapSize.y / 2)
                return InGameMapCell.GetEmptyCell();

            int gridX = Mathf.FloorToInt((pos.x + FullMapSize.x / 2) / CellUnitSize.x);
            int gridY = Mathf.FloorToInt((pos.y + FullMapSize.y / 2) / CellUnitSize.y);
            return new InGameMapCell(gridX, gridY, CellUnitSize, FullMapSize);
        }
    }
}