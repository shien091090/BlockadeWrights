using UnityEngine;

namespace GameCore
{
    public class MonsterScriptableObject : ScriptableObject, IMonsterSetting
    {
        [SerializeField] private float hp;
        [SerializeField] private float moveSpeed;

        public float GetHp => hp;
        public float GetMoveSpeed => moveSpeed;
    }
}