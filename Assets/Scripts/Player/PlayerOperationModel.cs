using System;

namespace GameCore
{
    public class PlayerOperationModel : IPlayerOperationModel
    {
        private readonly IInputKeyController inputKeyController;

        public PlayerOperationModel(IInputKeyController inputKeyController)
        {
            this.inputKeyController = inputKeyController;
        }

        public event Action<IInGameMapCell> OnCreateBuilding;

        public bool CreateBuilding(IInGameMapCell targetMapCell)
        {
            if (targetMapCell.IsEmpty == false)
                OnCreateBuilding?.Invoke(targetMapCell);

            return targetMapCell.IsEmpty == false;
        }

        public void UpdateCheckBuild(IInGameMapCell cell)
        {
            if (inputKeyController.GetBuildKeyDown())
                CreateBuilding(cell);
        }
    }
}