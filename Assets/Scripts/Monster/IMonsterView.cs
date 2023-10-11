using UnityEngine;

namespace GameCore
{
    public interface IMonsterView : IFaceDirectionView
    {
        ITransform GetTransform { get; }
        string GetId { get; }
        void InitSprite(Sprite frontSide, Sprite backSide);
        void SetupHp(HealthPointModel monsterModelHpModel);
        void SetActive(bool isActive);
        void Bind(MonsterModel model);
    }
}