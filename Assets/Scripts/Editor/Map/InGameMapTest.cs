using NUnit.Framework;
using UnityEngine;

namespace GameCore.Tests.Map
{
    public class InGameMapTest
    {
        private InGameMapModel mapModel;

        [SetUp]
        public void Setup()
        {
            mapModel = null;
        }

        [Test]
        //地圖尺寸為0
        public void map_size_is_zero()
        {
            GivenMapModel(new Vector2(0, 0), new Vector2(0, 0));

            InGameMapCell cell = mapModel.GetCellByPosition(new Vector2(100, 100));

            CellShouldBeEmpty(cell, true);
        }

        [Test]
        //在地圖內(第一格, x=0, y=0)
        public void inside_map_first_cell()
        {
            GivenMapModel(new Vector2(10, 10), new Vector2(1, 1));

            InGameMapCell cell = mapModel.GetCellByPosition(new Vector2(0, 0));

            CellShouldBeEmpty(cell, false);
            CellPositionShouldBe(cell, 0, 0);
        }

        [Test]
        //在地圖內(未定義格子尺寸)
        public void inside_map_undefined_cell_size()
        {
            GivenMapModel(new Vector2(10, 10), new Vector2(0, 0));

            InGameMapCell cell = mapModel.GetCellByPosition(new Vector2(5, 5));

            CellShouldBeEmpty(cell, true);
        }

        [Test]
        [TestCase(2.5f, 2.5f, 2, 2)]
        [TestCase(0.7f, 3.1f, 0, 3)]
        [TestCase(9.9f, 0.01f, 9, 0)]
        [TestCase(9.9f, 9.1f, 9, 9)]
        //在地圖內(格子尺寸為整數)
        public void inside_map_cell_size_is_integer(float posX, float posY, int expectedGridX, int expectedGridY)
        {
            GivenMapModel(new Vector2(10, 10), new Vector2(1, 1));

            InGameMapCell cell = mapModel.GetCellByPosition(new Vector2(posX, posY));

            CellShouldBeEmpty(cell, false);
            CellPositionShouldBe(cell, expectedGridX, expectedGridY);
        }

        [Test]
        [TestCase(0.6f, 0.1f, 1, 0)]
        //在地圖內(格子尺寸為小數)
        public void inside_map_cell_size_is_float(float posX, float posY, int expectedGridX, int expectedGridY)
        {
            GivenMapModel(new Vector2(10, 10), new Vector2(0.5f, 0.5f));

            InGameMapCell cell = mapModel.GetCellByPosition(new Vector2(posX, posY));

            CellPositionShouldBe(cell, expectedGridX, expectedGridY);
        }

        private void GivenMapModel(Vector2 mapSize, Vector2 cellSize)
        {
            mapModel = new InGameMapModel(mapSize, cellSize);
        }

        private void CellPositionShouldBe(InGameMapCell cell, int expectedPosX, int expectedPosY)
        {
            Assert.AreEqual(expectedPosX, cell.GridPosition.x);
            Assert.AreEqual(expectedPosY, cell.GridPosition.y);
        }

        private void CellShouldBeEmpty(InGameMapCell cell, bool expectedIsEmpty)
        {
            Assert.AreEqual(expectedIsEmpty, cell.IsEmpty);
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