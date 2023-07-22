using NUnit.Framework;
using UnityEngine;

namespace GameCore.Tests.Map
{
    public class InGameMapTest
    {
        [Test]
        //地圖尺寸為0
        public void map_size_is_zero()
        {
            InGameMapModel mapModel = new InGameMapModel(new Vector2(0, 0), new Vector2(0, 0));
            InGameMapCell cell = mapModel.GetCellByPosition(new Vector2(100, 100));
            Assert.IsTrue(cell.IsEmpty);
        }

        [Test]
        //在地圖內(第一格, x=0, y=0)
        public void inside_map_first_cell()
        {
            InGameMapModel mapModel = new InGameMapModel(new Vector2(10, 10), new Vector2(1, 1));
            InGameMapCell cell = mapModel.GetCellByPosition(new Vector2(0, 0));
            Assert.IsFalse(cell.IsEmpty);
            Assert.AreEqual(0, cell.GridPosition.x);
            Assert.AreEqual(0, cell.GridPosition.y);
        }

        [Test]
        //在地圖內(未定義格子尺寸)
        public void inside_map_undefined_cell_size()
        {
            InGameMapModel mapModel = new InGameMapModel(new Vector2(10, 10), new Vector2(0, 0));
            InGameMapCell cell = mapModel.GetCellByPosition(new Vector2(5, 5));
            Assert.IsTrue(cell.IsEmpty);
        }
        
        [Test]
        //在地圖內(格子尺寸為整數)
        public void inside_map_cell_size_is_integer()
        {
            InGameMapModel mapModel = new InGameMapModel(new Vector2(10, 10), new Vector2(1, 1));
            InGameMapCell cell = mapModel.GetCellByPosition(new Vector2(2.5f, 2.5f));
            Assert.IsFalse(cell.IsEmpty);
            Assert.AreEqual(2, cell.GridPosition.x);
            Assert.AreEqual(2, cell.GridPosition.y);
        }

        // [Test]
        // //在地圖外
        // public void outside_map()
        // {
        //     InGameMapModel mapModel = new InGameMapModel(new Vector2(10, 10));
        //     InGameMapCell cell = mapModel.GetCellByPosition(new Vector2(100, 100));
        //     Assert.IsTrue(cell.IsEmpty);
        // }

        //未定義地圖
    }
}