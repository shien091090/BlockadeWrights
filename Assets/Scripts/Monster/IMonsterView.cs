using UnityEngine;

namespace GameCore
{
    public interface IMonsterView
    {
        ITransform GetTransform { get; }
        string GetId { get; }
        void InitSprite(Sprite frontSide, Sprite backSide);
        void SetupHp(HealthPointModel monsterModelHpModel);
        void SetActive(bool isActive);
        void RefreshFaceDirection(FaceDirectionState faceDirectionState);
        void Bind(MonsterModel model);
    }
}