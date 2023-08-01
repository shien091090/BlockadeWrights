using NUnit.Framework;
using UnityEngine;

namespace GameCore.Tests.Monster
{
    public class MonsterModelTest
    {
        private MonsterModel monsterModel;

        [Test]
        //無行走路線, 不移動
        public void move_no_path()
        {
            GivenPath();

            Vector2 moveVector = monsterModel.UpdateMove(new Vector2(0, 0), 1, 1);

            ShouldNoMove(moveVector);
        }

        [Test]
        //行走路徑僅起點和終點, 往終點移動
        public void move_path_start_to_end()
        {
            GivenPath(
                new Vector2(0, 0),
                new Vector2(10, -10));

            Vector2 moveVector = monsterModel.UpdateMove(new Vector2(0, 0), 1, 1);

            ShouldMoveRightAndDown(moveVector);
        }

        [Test]
        //行走路徑有多個點, 從中間點往下一個點移動
        public void move_path_middle_to_next()
        {
            GivenPath(
                new Vector2(0, 0),
                new Vector2(10, -10),
                new Vector2(10, 0),
                new Vector2(50, 50));

            GivenTargetPathIndex(2);

            Vector2 moveVector = monsterModel.UpdateMove(new Vector2(10, -10), 1, 1);

            ShouldMoveUp(moveVector);
        }

        private void GivenTargetPathIndex(int index)
        {
            monsterModel.SetTargetPathIndex(index);
        }

        private void GivenPath(params Vector2[] pathPoints)
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

        //抵達第二個點, 轉向往第三個點移動
        //移動至終點, 破壞主堡
    }
}