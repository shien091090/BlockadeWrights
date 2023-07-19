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
        //角色未曾移動時面向下右
        public void face_down_right_when_not_move()
        {
            FaceDirectionShouldBe(FaceDirection.DownAndRight);
        }
        
        [Test]
        [TestCase(0.7f, 0.7f, FaceDirection.UpAndRight)]
        [TestCase(-0.7f, 0.7f, FaceDirection.UpAndLeft)]
        [TestCase(0.7f, -0.7f, FaceDirection.DownAndRight)]
        [TestCase(-0.7f, -0.7f, FaceDirection.DownAndLeft)]
        //角色斜角移動時面向行走方向
        public void face_direction_when_move(float vectorX, float vectorY, FaceDirection expectedFaceDir)
        {
            GivenMoveAxis(vectorX, vectorY);
            playerModel.UpdateMove(1, 1);
            FaceDirectionShouldBe(expectedFaceDir);
        }

        private void GivenMoveAxis(float horizontalAxis, float verticalAxis)
        {
            inputAxisController.GetHorizontalAxis().Returns(horizontalAxis);
            inputAxisController.GetVerticalAxis().Returns(verticalAxis);
        }

        private void FaceDirectionShouldBe(FaceDirection expectedFaceDir)
        {
            Assert.AreEqual(expectedFaceDir, playerModel.PlayerFaceDir);
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