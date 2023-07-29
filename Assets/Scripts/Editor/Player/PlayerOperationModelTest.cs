using NSubstitute;
using NUnit.Framework;

namespace GameCore.Tests.Player
{
    public class PlayerOperationModelTest
    {
        private PlayerOperationModel playerOperationModel;
        private IInGameMapCell targetMapCell;

        [SetUp]
        public void Setup()
        {
            playerOperationModel = new PlayerOperationModel();
            targetMapCell = Substitute.For<IInGameMapCell>();
        }

        [Test]
        //角色在無效格子上蓋塔
        public void build_on_invalid_cell()
        {
            GivenCellIsEmpty(true);

            ShouldAbleCreateBuilding(false);
        }

        [Test]
        //角色在有效格子上蓋塔
        public void build_on_valid_cell()
        {
            GivenCellIsEmpty(false);

            ShouldAbleCreateBuilding(true);
        }

        private void GivenCellIsEmpty(bool isEmpty)
        {
            targetMapCell.IsEmpty.Returns(isEmpty);
        }

        private void ShouldAbleCreateBuilding(bool expectedAbleCreate)
        {
            Assert.AreEqual(expectedAbleCreate, playerOperationModel.CreateBuilding(targetMapCell));
        }
    }
}