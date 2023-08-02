using UnityEngine;

namespace GameCore
{
    public interface IGameObjectPoolEntity
    {
        void Show();
        void Hide();
        bool IsActive { get; }
        void SetPos(Vector2 pos);
    }
}