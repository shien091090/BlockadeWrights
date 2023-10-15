using System;

namespace GameCore
{
    public interface IFortressModel
    {
        event Action OnFortressDestroy;
        float CurrentHp { get; }
        void Damage();
    }
}