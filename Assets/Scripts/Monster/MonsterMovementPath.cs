using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class MonsterMovementPath
    {
        private readonly List<Vector2> pathPointList;

        public bool IsEmpty => pathPointList.Count == 0;

        public int GetLastPointIndex => IsEmpty ?
            -1 :
            pathPointList.Count - 1;

        public MonsterMovementPath()
        {
            pathPointList = new List<Vector2>();
        }

        public Vector2 GetPoint(int index)
        {
            return index >= pathPointList.Count ?
                pathPointList[pathPointList.Count - 1] :
                pathPointList[index];
        }

        public void AddPoint(Vector2 pathPoint)
        {
            pathPointList.Add(pathPoint);
        }
    }
}