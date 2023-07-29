using System;

namespace GameCore.Tests.Player
{
    public class PlayerOperationModel : IPlayerOperationModel
    {
        public event Action<IInGameMapCell> OnCreateBuilding;

        public bool CreateBuilding(IInGameMapCell targetMapCell)
        {
            if (targetMapCell.IsEmpty == false)
                OnCreateBuilding?.Invoke(targetMapCell);

            return targetMapCell.IsEmpty == false;
        }
    }
}