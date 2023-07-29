using UnityEngine;

namespace GameCore
{
    public interface IInGameMapSetting
    {
        Vector2 MapSize { get; }
        Vector2 CellSize { get; }
    }
}