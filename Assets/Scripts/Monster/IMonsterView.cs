using UnityEngine;

namespace GameCore
{
    public interface IMonsterView : IFaceDirectionView
    {
        ITransform GetTransform { get; }
        string GetId { get; }
        IHealthPointView GetHealthPointView { get; }
        void InitSprite(Sprite frontSide, Sprite backSide);
        void SetActive(bool isActive);
        void Bind(MonsterModel model);
    }
}