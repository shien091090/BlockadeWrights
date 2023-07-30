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
        private IInputKeyController inputKeyController;

        [SetUp]
        public void Setup()
        {
            inputKeyController = Substitute.For<IInputKeyController>();
            playerOperationModel = new PlayerOperationModel(inputKeyController);
            targetMapCell = Substitute.For<IInGameMapCell>();

            onCreateBuilding = Substitute.For<Action<IInGameMapCell>>();
            playerOperationModel.OnCreateBuilding += onCreateBuilding;
        }

        [Test]
        //角色在無效格子上蓋塔
        public void build_on_invalid_cell()
        {
            GivenCellIsEmpty(true);
            GivenClickBuildKey();

            playerOperationModel.UpdateCheckBuild(targetMapCell);

            ShouldTriggerCreateBuildingEvent(0);
        }

        [Test]
        //角色在有效格子上蓋塔
        public void build_on_valid_cell()
        {
            GivenCellIsEmpty(false);
            GivenClickBuildKey();

            playerOperationModel.UpdateCheckBuild(targetMapCell);

            ShouldTriggerCreateBuildingEvent(1);
        }

        private void GivenClickBuildKey()
        {
            inputKeyController.GetBuildKeyDown().Returns(true);
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
    }
}