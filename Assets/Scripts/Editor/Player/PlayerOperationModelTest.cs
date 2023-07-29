using System;
using NSubstitute;
using NUnit.Framework;

namespace GameCore.Tests.Player
{
    public class PlayerOperationModelTest
    {
        private PlayerOperationModel playerOperationModel;
        private IInGameMapCell targetMapCell;
        private Action<IInGameMapCell> onCreateBuilding;

        [SetUp]
        public void Setup()
        {
            playerOperationModel = new PlayerOperationModel();
            targetMapCell = Substitute.For<IInGameMapCell>();

            onCreateBuilding = Substitute.For<Action<IInGameMapCell>>();
            playerOperationModel.OnCreateBuilding += onCreateBuilding;
        }

        [Test]
        //角色在無效格子上蓋塔
        public void build_on_invalid_cell()
        {
            GivenCellIsEmpty(true);

            ShouldAbleCreateBuilding(false);
            ShouldTriggerCreateBuildingEvent(0);
        }

        [Test]
        //角色在有效格子上蓋塔
        public void build_on_valid_cell()
        {
            GivenCellIsEmpty(false);

            ShouldAbleCreateBuilding(true);
            ShouldTriggerCreateBuildingEvent(1);
        }

        private void GivenCellIsEmpty(bool isEmpty)
        {
            targetMapCell.IsEmpty.Returns(isEmpty);
        }

        private void ShouldTriggerCreateBuildingEvent(int triggerTimes)
        {
            if (triggerTimes == 0)
                onCreateBuilding.DidNotReceive().Invoke(Arg.Any<IInGameMapCell>());
            else
                onCreateBuilding.Received(triggerTimes).Invoke(Arg.Any<IInGameMapCell>());
        }

        private void ShouldAbleCreateBuilding(bool expectedAbleCreate)
        {
            Assert.AreEqual(expectedAbleCreate, playerOperationModel.CreateBuilding(targetMapCell));
        }
    }
}