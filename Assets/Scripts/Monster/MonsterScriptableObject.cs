using Sirenix.OdinInspector;
using UnityEngine;

namespace GameCore
{
    public class MonsterScriptableObject : SerializedScriptableObject, IMonsterSetting
    {
        [SerializeField] private float hp;
        [SerializeField] private float moveSpeed;

        [SerializeField] [PreviewField] [HorizontalGroup("MonsterSprite")]
        private Sprite frontSideSprite;

        [SerializeField] [PreviewField] [HorizontalGroup("MonsterSprite")]
        private Sprite backSideSprite;

        public float GetHp => hp;
        public float GetMoveSpeed => moveSpeed;
        public Sprite GetFrontSideSprite => frontSideSprite;
        public Sprite GetBackSideSprite => backSideSprite;
        public Sprite GetSprite => frontSideSprite;
    }
}