using NUnit.Framework;
using UnityEngine;

namespace GameCore.Tests.Player
{
    public class PlayerModelTest
    {
        private PlayerModel playerModel;

        [SetUp]
        public void Setup()
        {
            playerModel = new PlayerModel();
        }

        [Test]
        //角色不移動
        public void no_move()
        {
            Vector2 moveVector = playerModel.UpdateMove(Vector2.zero, 1, 1);
            Assert.IsTrue(moveVector == Vector2.zero);
        }

        [Test]
        //角色水平移動
        public void horizontal_move()
        {
            Vector2 moveVector = playerModel.UpdateMove(Vector2.right, 1, 1);
            Assert.IsTrue(moveVector.x > 0);
            Assert.IsTrue(moveVector.y == 0);
        }

        [Test]
        //角色垂直移動
        public void vertical_move()
        {
            Vector2 moveVector = playerModel.UpdateMove(Vector2.up, 1, 1);
            Assert.IsTrue(moveVector.y > 0);
            Assert.IsTrue(moveVector.x == 0);
        }

        [Test]
        //角色水平+垂直移動
        public void horizontal_vertical_move()
        {
            Vector2 moveVector = playerModel.UpdateMove(-Vector2.one, 1, 1);
            Assert.IsTrue(moveVector.x < 0);
            Assert.IsTrue(moveVector.y < 0);
        }
    }
}