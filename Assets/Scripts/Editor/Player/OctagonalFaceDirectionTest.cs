using System;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace GameCore.Tests.Player.FaceDirection
{
    public class OctagonalFaceDirectionTest
    {
        private GameCore.FaceDirection faceDirection;
        private IFaceDirectionView faceDirectionView;

        [SetUp]
        public void Setup()
        {
            faceDirection = null;
            faceDirectionView = Substitute.For<IFaceDirectionView>();
        }

        [Test]
        [TestCase(FaceDirectionState.Down)]
        [TestCase(FaceDirectionState.UpAndLeft)]
        //驗證初始面向
        public void face_down_right_when_init(FaceDirectionState startFaceDir)
        {
            faceDirection = new GameCore.FaceDirection(new OctagonalDirectionStrategy(), startFaceDir);

            FaceDirectionShouldBe(startFaceDir);
            ShouldNotRefreshFaceDirectionView();
        }

        [Test]
        [TestCase(0, 0.7f, FaceDirectionState.Up)]
        [TestCase(0.7f, 0, FaceDirectionState.Right)]
        [TestCase(0, -0.2f, FaceDirectionState.Down)]
        [TestCase(-0.4f, 0, FaceDirectionState.Left)]
        [TestCase(0.7f, 0.7f, FaceDirectionState.UpAndRight)]
        [TestCase(-0.4f, 0.7f, FaceDirectionState.UpAndLeft)]
        [TestCase(0.8f, -0.2f, FaceDirectionState.DownAndRight)]
        [TestCase(-0.9f, -0.1f, FaceDirectionState.DownAndLeft)]
        //行走方向和面向一致
        public void face_direction_is_no_change_when_move_direction_is_same(float moveVectorX, float moveVectorY, FaceDirectionState expectedFaceDir)
        {
            faceDirection = new GameCore.FaceDirection(new OctagonalDirectionStrategy(), expectedFaceDir);
            faceDirection.BindView(faceDirectionView);
            faceDirection.MoveToChangeFaceDirection(new Vector2(moveVectorX, moveVectorY));

            FaceDirectionShouldBe(expectedFaceDir);
            ShouldNotRefreshFaceDirectionView();
        }

        [Test]
        [TestCase(FaceDirectionState.Up, -0.5f, 0.5f, FaceDirectionState.UpAndLeft)]
        [TestCase(FaceDirectionState.Up, 0, -0.5f, FaceDirectionState.Down)]
        [TestCase(FaceDirectionState.UpAndLeft, -0.5f, 0, FaceDirectionState.Left)]
        [TestCase(FaceDirectionState.UpAndLeft, 0.5f, -0.5f, FaceDirectionState.DownAndRight)]
        [TestCase(FaceDirectionState.Right, 0.5f, -0.5f, FaceDirectionState.DownAndRight)]
        //行走方向和面向不一致
        public void face_direction_is_change_when_move_direction_is_different(FaceDirectionState startFaceDir, float moveVectorX, float moveVectorY,
            FaceDirectionState expectedFaceDir)
        {
            faceDirection = new GameCore.FaceDirection(new OctagonalDirectionStrategy(), startFaceDir);
            faceDirection.BindView(faceDirectionView);
            faceDirection.MoveToChangeFaceDirection(new Vector2(moveVectorX, moveVectorY));

            FaceDirectionShouldBe(expectedFaceDir);
            ShouldRefreshFaceDirectionView(expectedFaceDir);
        }

        private void ShouldNotRefreshFaceDirectionView()
        {
            faceDirectionView.DidNotReceive().RefreshFaceDirection(Arg.Any<FaceDirectionState>());
        }

        private void ShouldRefreshFaceDirectionView(FaceDirectionState expectedFaceDir, int callTimes = 1)
        {
            faceDirectionView.Received(callTimes).RefreshFaceDirection(expectedFaceDir);
        }

        private void FaceDirectionShouldBe(FaceDirectionState expectedFaceDir)
        {
            Assert.AreEqual(expectedFaceDir, faceDirection.CurrentFaceDirectionState);
        }
    }
}