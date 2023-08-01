using NUnit.Framework;
using UnityEngine;

namespace GameCore.Tests.Monster
{
    public class MonsterModelTest
    {
        [Test]
        //無行走路線, 不移動
        public void move_no_path()
        {
            MonsterMovementPath path = new MonsterMovementPath();
            MonsterModel monsterModel = new MonsterModel(path);
            Vector2 moveVector = monsterModel.UpdateMove(1, 1);
            Assert.AreEqual(0, moveVector.x);
            Assert.AreEqual(0, moveVector.y);
        }

        [Test]
        //行走路徑僅起點和終點, 往終點移動
        public void move_path_start_to_end()
        {
            MonsterMovementPath path = new MonsterMovementPath();
            path.AddPoint(new Vector2(0, 0));
            path.AddPoint(new Vector2(10, -10));

            MonsterModel monsterModel = new MonsterModel(path);
            Vector2 moveVector = monsterModel.UpdateMove(1, 1);
            Assert.AreEqual(true, moveVector.x > 0);
            Assert.AreEqual(false, moveVector.y > 0);
        }

        //行走路徑有多個點, 從起點往第二個點移動
        //抵達第二個點, 轉向往第三個點移動
        //行走路徑有多個點, 從中間點往下一個點移動
        //移動至終點, 破壞主堡
    }
}