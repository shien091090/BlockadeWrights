namespace GameCore.Tests.Player
{
    public class PlayerOperationModel
    {
        public bool CreateBuilding(IInGameMapCell targetMapCell)
        {
            return targetMapCell.IsEmpty == false;
        }
    }
}