using System;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace GameCore.Tests.Player.FaceDirection
{
    public class QuadrantFaceDirectionTest
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
        [TestCase(FaceDirectionState.DownAndLeft)]
        [TestCase(FaceDirectionState.UpAndRight)]
        //驗證初始面向
        public void face_down_right_when_init(FaceDirectionState startFaceDir)
        {
            faceDirection = new GameCore.FaceDirection(new QuadrantDirectionStrategy(), startFaceDir);

            FaceDirectionShouldBe(startFaceDir);
            ShouldNotRefreshFaceDirectionView();
        }

        [Test]
        [TestCase(0.7f, 0.7f, FaceDirectionState.UpAndRight)]
        [TestCase(-0.7f, 0.7f, FaceDirectionState.UpAndLeft)]
        [TestCase(0.7f, -0.7f, FaceDirectionState.DownAndRight)]
        [TestCase(-0.7f, -0.7f, FaceDirectionState.DownAndLeft)]
        //角色斜角移動時面向行走方向
        public void face_direction_when_move(float vectorX, float vectorY, FaceDirectionState expectedFaceDir)
        {
            faceDirection = new GameCore.FaceDirection(new QuadrantDirectionStrategy());
            faceDirection.BindView(faceDirectionView);
            faceDirection.MoveToChangeFaceDirection(new Vector2(vectorX, vectorY));

            FaceDirectionShouldBe(expectedFaceDir);
            ShouldRefreshFaceDirectionView(expectedFaceDir);
        }

        [Test]
        [TestCase(0.7f, 0.7f, FaceDirectionState.UpAndRight)]
        [TestCase(0.7f, 0, FaceDirectionState.UpAndRight)]
        [TestCase(0, 0.7f, FaceDirectionState.UpAndRight)]
        [TestCase(-0.4f, 0.7f, FaceDirectionState.UpAndLeft)]
        [TestCase(-0.4f, 0, FaceDirectionState.UpAndLeft)]
        [TestCase(0, 0.7f, FaceDirectionState.UpAndLeft)]
        [TestCase(0.8f, -0.2f, FaceDirectionState.DownAndRight)]
        [TestCase(0.8f, 0, FaceDirectionState.DownAndRight)]
        [TestCase(0, -0.2f, FaceDirectionState.DownAndRight)]
        [TestCase(-0.9f, -0.1f, FaceDirectionState.DownAndLeft)]
        [TestCase(0, -0.1f, FaceDirectionState.DownAndLeft)]
        [TestCase(-0.9f, 0, FaceDirectionState.DownAndLeft)]
        //行走方向和面向一致或部分一致
        public void face_direction_is_no_change_when_move_direction_is_same_or_partially_same(float moveVectorX, float moveVectorY, FaceDirectionState expectedFaceDir)
        {
            faceDirection = new GameCore.FaceDirection(new QuadrantDirectionStrategy(), expectedFaceDir);
            faceDirection.BindView(faceDirectionView);
            faceDirection.MoveToChangeFaceDirection(new Vector2(moveVectorX, moveVectorY));

            FaceDirectionShouldBe(expectedFaceDir);
            ShouldNotRefreshFaceDirectionView();
        }

        [Test]
        [TestCase(FaceDirectionState.UpAndRight, -0.5f, 0.5f, FaceDirectionState.UpAndLeft)]
        [TestCase(FaceDirectionState.UpAndRight, 0.5f, -0.5f, FaceDirectionState.DownAndRight)]
        [TestCase(FaceDirectionState.UpAndLeft, 0.5f, 0.5f, FaceDirectionState.UpAndRight)]
        [TestCase(FaceDirectionState.UpAndLeft, -0.5f, -0.5f, FaceDirectionState.DownAndLeft)]
        [TestCase(FaceDirectionState.DownAndRight, -0.5f, -0.5f, FaceDirectionState.DownAndLeft)]
        [TestCase(FaceDirectionState.DownAndRight, 0.5f, 0.5f, FaceDirectionState.UpAndRight)]
        [TestCase(FaceDirectionState.DownAndLeft, 0.5f, -0.5f, FaceDirectionState.DownAndRight)]
        [TestCase(FaceDirectionState.DownAndLeft, -0.5f, 0.5f, FaceDirectionState.UpAndLeft)]
        //行走方向和面向不一致
        public void face_direction_is_change_when_move_direction_is_different(FaceDirectionState startFaceDir, float moveVectorX, float moveVectorY,
            FaceDirectionState expectedFaceDir)
        {
            faceDirection = new GameCore.FaceDirection(new QuadrantDirectionStrategy(), startFaceDir);
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