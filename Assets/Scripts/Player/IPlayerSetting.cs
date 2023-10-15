using UnityEngine;

namespace GameCore
{
    public interface IPlayerSetting
    {
        float MoveSpeed { get; }
        Vector2 TouchRange { get; }
        float FortressHp { get; }
    }
}