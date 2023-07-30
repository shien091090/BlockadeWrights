using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace GameCore.Tests.Player
{
    public class PlayerModelTest
    {
        private PlayerModel playerModel;
        private IInputAxisController inputAxisController;
        private IInputKeyController inputKeyController;
        private IInGameMapModel inGameMapModel;
        private IPlayerOperationModel playerOperationModel;

        [SetUp]
        public void Setup()
        {
            inputAxisController = Substitute.For<IInputAxisController>();
            inputKeyController = Substitute.For<IInputKeyController>();
            inGameMapModel = Substitute.For<IInGameMapModel>();
            playerOperationModel = Substitute.For<IPlayerOperationModel>();

            playerModel = new PlayerModel(inputAxisController, inputKeyController, inGameMapModel, playerOperationModel);
        }

        [Test]
        //角色不移動
        public void no_move()
        {
            GivenMoveAxis(0, 0);
            Vector2 moveVector = playerModel.UpdateMove(1, 1);
            ShouldNoMove(moveVector);
        }

        [Test]
        //角色水平移動
        public void horizontal_move()
        {
            GivenMoveAxis(0.6f, 0);
            Vector2 moveVector = playerModel.UpdateMove(1, 1);
            ShouldMoveRight(moveVector);
        }

        [Test]
        //角色垂直移動
        public void vertical_move()
        {
            GivenMoveAxis(0, 0.7f);
            Vector2 moveVector = playerModel.UpdateMove(1, 1);
            ShouldMoveUp(moveVector);
        }

        [Test]
        //角色水平+垂直移動
        public void horizontal_vertical_move()
        {
            GivenMoveAxis(-0.7f, -0.8f);
            Vector2 moveVector = playerModel.UpdateMove(1, 1);
            ShouldMoveLeftAndDown(moveVector);
        }

        [Test]
        //按下建造按鍵
        public void press_build_key()
        {
            InGameMapCell emptyCell = InGameMapCell.GetEmptyCell();
            inputKeyController.GetBuildKeyDown().Returns(true);
            playerModel.UpdateCheckBuild(emptyCell);
            playerOperationModel.Received(1).CreateBuilding(Arg.Any<IInGameMapCell>());
        }

        private void GivenMoveAxis(float horizontalAxis, float verticalAxis)
        {
            inputAxisController.GetHorizontalAxis().Returns(horizontalAxis);
            inputAxisController.GetVerticalAxis().Returns(verticalAxis);
        }

        private void ShouldMoveLeftAndDown(Vector2 moveVector)
        {
            Assert.IsTrue(moveVector.x < 0);
            Assert.IsTrue(moveVector.y < 0);
        }

        private void ShouldMoveUp(Vector2 moveVector)
        {
            Assert.IsTrue(moveVector.y > 0);
            Assert.IsTrue(moveVector.x == 0);
        }

        private void ShouldMoveRight(Vector2 moveVector)
        {
            Assert.IsTrue(moveVector.x > 0);
            Assert.IsTrue(moveVector.y == 0);
        }

        private void ShouldNoMove(Vector2 moveVector)
        {
            Assert.IsTrue(moveVector == Vector2.zero);
        }
    }
}