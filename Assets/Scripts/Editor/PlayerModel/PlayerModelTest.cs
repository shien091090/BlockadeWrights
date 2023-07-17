using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace GameCore.Tests.Player
{
    public class PlayerModelTest
    {
        private PlayerModel playerModel;
        private IInputAxisController inputAxisController;

        [SetUp]
        public void Setup()
        {
            inputAxisController = Substitute.For<IInputAxisController>();
            playerModel = new PlayerModel(inputAxisController);
        }

        [Test]
        //角色不移動
        public void no_move()
        {
            inputAxisController.GetHorizontalAxis().Returns(0);
            inputAxisController.GetVerticalAxis().Returns(0);
            Vector2 moveVector = playerModel.UpdateMove(1, 1);
            Assert.IsTrue(moveVector == Vector2.zero);
        }

        [Test]
        //角色水平移動
        public void horizontal_move()
        {
            inputAxisController.GetHorizontalAxis().Returns(0.6f);
            inputAxisController.GetVerticalAxis().Returns(0);
            Vector2 moveVector = playerModel.UpdateMove(1, 1);
            Assert.IsTrue(moveVector.x > 0);
            Assert.IsTrue(moveVector.y == 0);
        }

        [Test]
        //角色垂直移動
        public void vertical_move()
        {
            inputAxisController.GetHorizontalAxis().Returns(0);
            inputAxisController.GetVerticalAxis().Returns(0.7f);
            Vector2 moveVector = playerModel.UpdateMove(1, 1);
            Assert.IsTrue(moveVector.y > 0);
            Assert.IsTrue(moveVector.x == 0);
        }

        [Test]
        //角色水平+垂直移動
        public void horizontal_vertical_move()
        {
            inputAxisController.GetHorizontalAxis().Returns(-0.7f);
            inputAxisController.GetVerticalAxis().Returns(-0.8f);
            Vector2 moveVector = playerModel.UpdateMove(1, 1);
            Assert.IsTrue(moveVector.x < 0);
            Assert.IsTrue(moveVector.y < 0);
        }
    }
}