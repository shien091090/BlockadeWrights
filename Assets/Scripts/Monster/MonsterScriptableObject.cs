using UnityEngine;

namespace GameCore
{
    public class MonsterScriptableObject : ScriptableObject, IMonsterSetting
    {
        [SerializeField] private float hp;
        
        public float GetHp => hp;
    }
}