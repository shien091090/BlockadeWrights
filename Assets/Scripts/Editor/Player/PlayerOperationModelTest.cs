using NUnit.Framework;

namespace GameCore.Tests.Player
{
    public class PlayerOperationModelTest
    {
        [Test]
        //角色在無效格子上蓋塔
        public void build_on_invalid_cell()
        {
            PlayerOperationModel playerOperationModel = new PlayerOperationModel();
            
            InGameMapCell emptyCell = InGameMapCell.GetEmptyCell();
            bool isCreateSuccess = playerOperationModel.CreateBuilding(emptyCell);

            Assert.AreEqual(false, isCreateSuccess);
        }
    }
}