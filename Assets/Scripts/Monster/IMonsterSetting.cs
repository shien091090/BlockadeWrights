using UnityEngine;

namespace GameCore
{
    public interface IMonsterSetting
    {
        float GetHp { get; }
        float GetMoveSpeed { get; }
        Sprite GetFrontSideSprite { get; }
        Sprite GetBackSideSprite { get; }
    }
}