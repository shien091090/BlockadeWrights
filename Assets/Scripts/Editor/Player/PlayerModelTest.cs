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