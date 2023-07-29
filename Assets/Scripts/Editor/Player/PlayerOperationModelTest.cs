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
            bool isCreateSuccess = playerOperationModel.CreateBuilding(5, 5);

            Assert.AreEqual(false, isCreateSuccess);
        }
    }
}