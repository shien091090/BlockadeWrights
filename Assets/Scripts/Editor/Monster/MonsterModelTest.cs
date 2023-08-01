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

            Vector2 moveVector = monsterModel.UpdateMove(1, 1);

            ShouldNoMove(moveVector);
        }

        [Test]
        //行走路徑僅起點和終點, 往終點移動
        public void move_path_start_to_end()
        {
            GivenPath(
                new Vector2(0, 0),
                new Vector2(10, -10));

            Vector2 moveVector = monsterModel.UpdateMove(1, 1);
            
            ShouldMoveRightAndDown(moveVector);
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

        private void ShouldMoveRightAndDown(Vector2 moveVector)
        {
            Assert.AreEqual(true, moveVector.x > 0);
            Assert.AreEqual(false, moveVector.y > 0);
        }

        private void ShouldNoMove(Vector2 moveVector)
        {
            Assert.AreEqual(0, moveVector.x);
            Assert.AreEqual(0, moveVector.y);
        }

        //行走路徑有多個點, 從起點往第二個點移動
        //抵達第二個點, 轉向往第三個點移動
        //行走路徑有多個點, 從中間點往下一個點移動
        //移動至終點, 破壞主堡
    }
}