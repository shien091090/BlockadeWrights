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
            InGameMapModel mapModel = new InGameMapModel(new Vector2(0, 0));
            InGameMapCell cell = mapModel.GetCellByPosition(new Vector2(100, 100));
            Assert.IsTrue(cell.IsEmpty);
        }
        
        //未定義地圖
        //在地圖外
        //在地圖內
    }
}