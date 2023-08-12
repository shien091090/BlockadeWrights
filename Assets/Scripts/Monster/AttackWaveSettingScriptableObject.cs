using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class AttackWaveSettingScriptableObject : ScriptableObject
    {
        [SerializeField] private List<Vector2> pathPointList;
        [SerializeField] private AttackWave[] attackWaves;
        
        public Vector2 StartPoint => pathPointList[0];
        public AttackWave[] GetAttackWaves => attackWaves;

        public MonsterMovementPath GetPathInfo()
        {
            MonsterMovementPath pathInfo = new MonsterMovementPath();
            foreach (Vector2 pos in pathPointList)
            {
                pathInfo.AddPoint(pos);
            }

            return pathInfo;
        }
    }
}