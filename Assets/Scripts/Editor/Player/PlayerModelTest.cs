using System.Linq;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace GameCore.Tests.Player
{
    public class PlayerModelTest
    {
        private PlayerModel playerModel;
        private IInputAxisController inputAxisController;
        private IInGameMapModel inGameMapModel;
        private IPlayerOperationModel playerOperationModel;
        private ITimeManager timeAdapter;
        private IPlayerView playerView;
        private ITransform transformAdapter;

        [SetUp]
        public void Setup()
        {
            inputAxisController = Substitute.For<IInputAxisController>();
            inGameMapModel = Substitute.For<IInGameMapModel>();
            playerOperationModel = Substitute.For<IPlayerOperationModel>();
            timeAdapter = Substitute.For<ITimeManager>();
            GivenDeltaTime(1);

            transformAdapter = Substitute.For<ITransform>();

            playerView = Substitute.For<IPlayerView>();
            playerView.GetTransform.Returns(transformAdapter);
            GivenMoveSpeed(1);
            GivenTouchRange(new Vector2(1, 1));

            playerModel = new PlayerModel(inputAxisController, inGameMapModel, playerOperationModel, timeAdapter);
            playerModel.Bind(playerView);
        }

        [Test]
        //角色不移動
        public void no_move()
        {
            GivenMoveAxis(0, 0);

            playerModel.Update();

            ShouldNoMove();
        }

        [Test]
        //角色水平移動
        public void horizontal_move()
        {
            GivenMoveAxis(0.6f, 0);

            playerModel.Update();

            ShouldMoveRight();
        }

        [Test]
        //角色垂直移動
        public void vertical_move()
        {
            GivenMoveAxis(0, 0.7f);

            playerModel.Update();

            ShouldMoveUp();
        }

        [Test]
        //角色水平+垂直移動
        public void horizontal_vertical_move()
        {
            GivenMoveAxis(-0.7f, -0.8f);

            playerModel.Update();

            ShouldMoveLeftAndDown();
        }

        [Test]
        //角色面對格子可建造建築, 顯示格子提示
        public void face_cell_can_build()
        {
            GivenMoveAxis(1, 0);
            GivenCurrentPosition(new Vector2(5, 5));
            GivenTouchRange(new Vector2(1, 1));
            GivenCellInfo(6, 5, new Vector2(10, 10));

            playerModel.Update();

            ShouldCallGetCellInfo(new Vector2(5, 5), FaceDirectionState.Right, new Vector2(1, 1));
            ShouldSetCellHintActive(true);
            ShouldSetCellHintPosition(new Vector2(1.5f, 0.5f));
        }

        [Test]
        //角色面對格子不可建造建築, 不顯示格子提示且不設定位置
        public void face_cell_cannot_build()
        {
            GivenMoveAxis(1, 0);
            GivenCurrentPosition(new Vector2(5, 5));
            GivenTouchRange(new Vector2(1, 1));
            GivenEmptyCellInfo();

            playerModel.Update();

            ShouldCallGetCellInfo(new Vector2(5, 5), FaceDirectionState.Right, new Vector2(1, 1));
            ShouldSetCellHintActive(false);
            ShouldNotSetCellHintPosition();
        }

        private void GivenEmptyCellInfo()
        {
            InGameMapCell cell = InGameMapCell.GetEmptyCell();
            inGameMapModel.GetCellInfo(Arg.Any<Vector2>(), Arg.Any<FaceDirectionState>(), Arg.Any<Vector2>()).Returns(cell);
        }

        private void GivenCellInfo(int gridX, int gridY, Vector2 fullMapSize)
        {
            InGameMapCell cell = new InGameMapCell(gridX, gridY, Vector2.one, fullMapSize);
            inGameMapModel.GetCellInfo(Arg.Any<Vector2>(), Arg.Any<FaceDirectionState>(), Arg.Any<Vector2>()).Returns(cell);
        }

        private void GivenCurrentPosition(Vector2 pos)
        {
            transformAdapter.Position.Returns(pos);
        }

        private void GivenTouchRange(Vector2 touchRange)
        {
            playerView.TouchRange.Returns(touchRange);
        }

        private void GivenMoveSpeed(int moveSpeed)
        {
            playerView.MoveSpeed.Returns(moveSpeed);
        }

        private void GivenDeltaTime(int deltaTime)
        {
            timeAdapter.DeltaTime.Returns(deltaTime);
        }

        private void GivenMoveAxis(float horizontalAxis, float verticalAxis)
        {
            inputAxisController.GetHorizontalAxis().Returns(horizontalAxis);
            inputAxisController.GetVerticalAxis().Returns(verticalAxis);
        }

        private void ShouldNotSetCellHintPosition()
        {
            playerView.DidNotReceive().SetCellHintPosition(Arg.Any<Vector2>());
        }

        private void ShouldSetCellHintPosition(Vector2 expectedPos)
        {
            playerView.Received(1).SetCellHintPosition(expectedPos);
        }

        private void ShouldSetCellHintActive(bool expectedIsActive)
        {
            playerView.Received(1).SetCellHintActive(expectedIsActive);
        }

        private void ShouldCallGetCellInfo(Vector2 expectedPos, FaceDirectionState expectedFaceDirectionType, Vector2 expectedTouchRange)
        {
            inGameMapModel.Received(1).GetCellInfo(expectedPos, expectedFaceDirectionType, expectedTouchRange);
        }

        private void ShouldMoveLeftAndDown()
        {
            Vector2 argument = (Vector2)transformAdapter.ReceivedCalls().Last(x => x.GetMethodInfo().Name == "Translate").GetArguments()[0];
            Assert.IsTrue(argument.x < 0);
            Assert.IsTrue(argument.y < 0);
        }

        private void ShouldMoveUp()
        {
            Vector2 argument = (Vector2)transformAdapter.ReceivedCalls().Last(x => x.GetMethodInfo().Name == "Translate").GetArguments()[0];
            Assert.IsTrue(argument.x == 0);
            Assert.IsTrue(argument.y > 0);
        }

        private void ShouldMoveRight()
        {
            Vector2 argument = (Vector2)transformAdapter.ReceivedCalls().Last(x => x.GetMethodInfo().Name == "Translate").GetArguments()[0];
            Assert.IsTrue(argument.x > 0);
            Assert.IsTrue(argument.y == 0);
        }

        private void ShouldNoMove()
        {
            transformAdapter.DidNotReceive().Translate(Arg.Any<Vector2>());
        }
    }
}