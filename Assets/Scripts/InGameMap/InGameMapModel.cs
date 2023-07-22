using UnityEngine;

namespace GameCore
{
    public class InGameMapModel
    {
        private Vector2 MapSize { get; }

        public InGameMapModel(Vector2 size)
        {
            MapSize = size;
        }

        public InGameMapCell GetCellByPosition(Vector3 worldPos)
        {
            if(MapSize.x == 0 || MapSize.y == 0)
                return new InGameMapCell(-1, -1);
            
            return new InGameMapCell(0, 0);
        }
    }
}