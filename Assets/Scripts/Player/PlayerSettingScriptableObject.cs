using UnityEngine;

namespace GameCore
{
    [CreateAssetMenu]
    public class PlayerSettingScriptableObject : ScriptableObject, IPlayerSetting
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Vector2 touchRange;
        [SerializeField] private float fortressHp;

        public float MoveSpeed => moveSpeed;
        public Vector2 TouchRange => touchRange;
        public float FortressHp => fortressHp;
    }
}