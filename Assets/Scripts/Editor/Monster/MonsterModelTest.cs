using System;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace GameCore.Tests.Monster
{
    public class MonsterModelTest
    {
        private MonsterModel monsterModel;
        private Action onDamageFort;

        [SetUp]
        public void Setup()
        {
            onDamageFort = Substitute.For<Action>();
        }

        [Test]
        //無行走路線, 不移動
        public void move_no_path()
        {
            GivenInitModel();

            Vector2 moveVector = monsterModel.UpdateMove(new Vector2(0, 0), 1, 1);

            ShouldNoMove(moveVector);
        }

        [Test]
        //行走路徑僅起點和終點, 往終點移動
        public void move_path_start_to_end()
        {
            GivenInitModel(
                new Vector2(0, 0),
                new Vector2(10, -10));

            Vector2 moveVector = monsterModel.UpdateMove(new Vector2(0, 0), 1, 1);

            ShouldMoveRightAndDown(moveVector);
        }

        [Test]
        //行走路徑有多個點, 從中間點往下一個點移動
        public void move_path_middle_to_next()
        {
            GivenInitModel(
                new Vector2(0, 0),
                new Vector2(10, -10),
                new Vector2(10, 0),
                new Vector2(50, 50));

            GivenTargetPathIndex(2);

            Vector2 moveVector = monsterModel.UpdateMove(new Vector2(10, -10), 1, 1);

            ShouldMoveUp(moveVector);
        }

        [Test]
        [TestCase(1, 9.9f, -9.9f)]
        [TestCase(2, 10.1f, -0.1f)]
        [TestCase(3, 49.9f, 49.9f)]
        //抵達後轉向至下一個點
        public void move_to_next_point_when_arrived(int startIndex, float currentPosX, float currentPosY)
        {
            GivenInitModel(
                new Vector2(0, 0),
                new Vector2(10, -10),
                new Vector2(10, 0),
                new Vector2(50, 50));

            GivenTargetPathIndex(startIndex);

            monsterModel.UpdateMove(new Vector2(currentPosX, currentPosY), 1.5f, 1);

            CurrentTargetPathIndexShouldBe(startIndex + 1);
        }

        [Test]
        //移動至終點, 破壞主堡
        public void move_to_end_point_and_destroy_main_castle()
        {
            GivenInitModel(
                new Vector2(0, 0),
                new Vector2(10, -10));

            monsterModel.UpdateMove(new Vector2(9.9f, -9.9f), 1, 1);

            ShouldBeArrivedGoal();
            ShouldBeTriggerDamageFortEvent(1);
        }

        private void GivenTargetPathIndex(int index)
        {
            monsterModel.SetTargetPathIndex(index);
        }

        private void GivenInitModel(params Vector2[] pathPoints)
        {
            MonsterMovementPath path = new MonsterMovementPath();
            if (pathPoints != null && pathPoints.Length > 0)
            {
                foreach (Vector2 pathPoint in pathPoints)
                {
                    path.AddPoint(pathPoint);
                }
            }

            monsterModel = new MonsterModel(path);
            monsterModel.OnDamageFort += onDamageFort;
        }

        private void ShouldBeTriggerDamageFortEvent(int triggerTimes)
        {
            if (triggerTimes == 0)
                onDamageFort.DidNotReceive().Invoke();
            else
                onDamageFort.Received(triggerTimes).Invoke();
        }

        private void ShouldBeArrivedGoal()
        {
            Assert.IsTrue(monsterModel.IsArrivedGoal);
        }

        private void CurrentTargetPathIndexShouldBe(int expectedIndex)
        {
            Assert.AreEqual(expectedIndex, monsterModel.CurrentTargetPathIndex);
        }

        private void ShouldMoveUp(Vector2 moveVector)
        {
            Assert.IsTrue(moveVector.y > 0);
            Assert.IsTrue(moveVector.x == 0);
        }

        private void ShouldMoveRightAndDown(Vector2 moveVector)
        {
            Assert.IsTrue(moveVector.x > 0);
            Assert.IsTrue(moveVector.y < 0);
        }

        private void ShouldNoMove(Vector2 moveVector)
        {
            Assert.AreEqual(0, moveVector.x);
            Assert.AreEqual(0, moveVector.y);
        }
    }
}