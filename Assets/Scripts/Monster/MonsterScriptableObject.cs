using UnityEngine;

namespace GameCore
{
    public class MonsterScriptableObject : ScriptableObject, IMonsterSetting
    {
        [SerializeField] private float hp;
        [SerializeField] private float moveSpeed;
        [SerializeField] private Sprite sprite;

        public float GetHp => hp;
        public float GetMoveSpeed => moveSpeed;
        public Sprite GetSprite => sprite;
    }
}