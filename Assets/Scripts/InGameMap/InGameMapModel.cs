using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class InGameMapModel
    {
        private readonly List<IntVector2> blockedCellList;
        public int GetBlockedCellCount => blockedCellList.Count;
        private Vector2 CellUnitSize { get; }
        private Vector2 FullMapSize { get; }
        private bool IsMapValid => FullMapSize.x == 0 || FullMapSize.y == 0;
        private bool IsCellUnitValid => CellUnitSize.x == 0 || CellUnitSize.y == 0;

        public InGameMapModel(Vector2 mapSize, Vector2 cellSize)
        {
            blockedCellList = new List<IntVector2>();
            FullMapSize = mapSize;
            CellUnitSize = cellSize;
        }

        public InGameMapCell GetCellInfo(Vector3 pos)
        {
            if (IsMapValid)
                return InGameMapCell.GetEmptyCell();

            if (IsCellUnitValid)
                return InGameMapCell.GetEmptyCell();

            if (IsOutOfMap(pos))
                return InGameMapCell.GetEmptyCell();

            int gridX = Mathf.FloorToInt((pos.x + FullMapSize.x / 2) / CellUnitSize.x);
            int gridY = Mathf.FloorToInt((pos.y + FullMapSize.y / 2) / CellUnitSize.y);

            if (IsInBlockedCell(new IntVector2(gridX, gridY)))
                return InGameMapCell.GetEmptyCell();

            return new InGameMapCell(gridX, gridY, CellUnitSize, FullMapSize);
        }

        public InGameMapCell GetCellInfo(Vector2 pos, FaceDirectionState faceDir, Vector2 touchRange = default)
        {
            Vector2 offsetBase = faceDir switch
            {
                FaceDirectionState.UpAndRight => new Vector2(1, 1),
                FaceDirectionState.UpAndLeft => new Vector2(-1, 1),
                FaceDirectionState.DownAndRight => new Vector2(1, -1),
                FaceDirectionState.DownAndLeft => new Vector2(-1, -1),
                FaceDirectionState.Up => new Vector2(0, 1),
                FaceDirectionState.Down => new Vector2(0, -1),
                FaceDirectionState.Right => new Vector2(1, 0),
                FaceDirectionState.Left => new Vector2(-1, 0),
                _ => Vector2.zero
            };

            Vector2 range = touchRange == default ?
                CellUnitSize :
                touchRange;

            Vector2 offset = new Vector2(offsetBase.x * range.x, offsetBase.y * range.y);
            return GetCellInfo(pos + offset);
        }

        public void SetCellBlocked(int gridX, int gridY)
        {
            IntVector2 pos = new IntVector2(gridX, gridY);

            if (blockedCellList.Contains(pos) == false)
                blockedCellList.Add(pos);
        }

        private bool IsInBlockedCell(IntVector2 pos)
        {
            return blockedCellList.Contains(pos);
        }

        private bool IsOutOfMap(Vector3 pos)
        {
            return pos.x >= FullMapSize.x / 2 || pos.y >= FullMapSize.y / 2 || pos.x < -FullMapSize.x / 2 || pos.y < -FullMapSize.y / 2;
        }
    }
}