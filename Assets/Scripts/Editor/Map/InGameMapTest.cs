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
        [TestCase(9.9f, 9.2f, 19, 18)]
        //在地圖內(格子尺寸為小數)
        public void inside_map_cell_size_is_float(float posX, float posY, int expectedGridX, int expectedGridY)
        {
            GivenMapModel(new Vector2(10, 10), new Vector2(0.5f, 0.5f));

            InGameMapCell cell = mapModel.GetCellByPosition(new Vector2(posX, posY));

            CellPositionShouldBe(cell, expectedGridX, expectedGridY);
        }

        [Test]
        [TestCase(0, 0, 0, 0)]
        [TestCase(0, 4, 0, 4)]
        [TestCase(9, 3, 9, 3)]
        //在地圖內(定位點在邊界上)
        public void position_is_on_the_edge(float posX, float posY, int expectedGridX, int expectedGridY)
        {
            GivenMapModel(new Vector2(10, 10), new Vector2(1, 1));

            InGameMapCell cell = mapModel.GetCellByPosition(new Vector2(posX, posY));

            CellPositionShouldBe(cell, expectedGridX, expectedGridY);
        }

        [Test]
        [TestCase(100, 100)]
        [TestCase(10, 10)]
        [TestCase(6, 10)]
        [TestCase(-0.1f, 3)]
        [TestCase(11, -1)]
        //在地圖外
        public void outside_map(float posX, float posY)
        {
            GivenMapModel(new Vector2(10, 10), new Vector2(1, 1));

            InGameMapCell cell = mapModel.GetCellByPosition(new Vector2(posX, posY));

            CellShouldBeEmpty(cell, true);
        }

        [Test]
        [TestCase(0.1f, 0.1f, 1.25f, 1.25f)]
        [TestCase(9.9f, 2.4f, 8.75f, 1.25f)]
        [TestCase(5, 7.5f, 6.25f, 8.75f)]
        //取得指定地圖格的中間位置
        public void get_cell_center_position(float posX, float posY, float centerX, float centerY)
        {
            GivenMapModel(new Vector2(10, 10), new Vector2(2.5f, 2.5f));

            InGameMapCell cell = mapModel.GetCellByPosition(new Vector2(posX, posY));

            Assert.AreEqual(centerX, cell.CenterPosition.x);
            Assert.AreEqual(centerY, cell.CenterPosition.y);
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
    }
}