using UnityEngine;

namespace GameCore
{
    public class MonsterScriptableObject : ScriptableObject, IMonsterSetting
    {
        [SerializeField] private float hp;
        [SerializeField] private float moveSpeed;
        [SerializeField] private Sprite frontSideSprite;
        [SerializeField] private Sprite backSideSprite;

        public float GetHp => hp;
        public float GetMoveSpeed => moveSpeed;
        public Sprite GetFrontSideSprite => frontSideSprite;
        public Sprite GetBackSideSprite => backSideSprite;
        public Sprite GetSprite => frontSideSprite;
    }
}