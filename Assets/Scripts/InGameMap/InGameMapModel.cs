using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class InGameMapModel : IInGameMapModel
    {
        private readonly List<IntVector2> blockedCellList;
        private IPlayerOperationModel playerOperationModel;
        public int GetBlockedCellCount => blockedCellList.Count;
        private Vector2 CellUnitSize { get; }
        private Vector2 FullMapSize { get; }
        private bool IsMapValid => FullMapSize.x == 0 || FullMapSize.y == 0;
        private bool IsCellUnitValid => CellUnitSize.x == 0 || CellUnitSize.y == 0;

        public InGameMapModel(IInGameMapSetting mapSetting, IPlayerOperationModel playerOperationModel)
        {
            this.playerOperationModel = playerOperationModel;

            blockedCellList = new List<IntVector2>();
            FullMapSize = mapSetting.MapSize;
            CellUnitSize = mapSetting.CellSize;

            RegisterEvent();
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

        public void SetCellBlocked(IntVector2 gridPos)
        {
            if (blockedCellList.Contains(gridPos) == false)
                blockedCellList.Add(gridPos);
        }

        private bool IsInBlockedCell(IntVector2 pos)
        {
            return blockedCellList.Contains(pos);
        }

        private bool IsOutOfMap(Vector3 pos)
        {
            return pos.x >= FullMapSize.x / 2 || pos.y >= FullMapSize.y / 2 || pos.x < -FullMapSize.x / 2 || pos.y < -FullMapSize.y / 2;
        }

        private void AddBlockedCell(IInGameMapCell targetMapCell)
        {
            SetCellBlocked(targetMapCell.GridPosition);
        }

        private void RegisterEvent()
        {
            playerOperationModel.OnCreateBuilding -= AddBlockedCell;
            playerOperationModel.OnCreateBuilding += AddBlockedCell;
        }
    }
}