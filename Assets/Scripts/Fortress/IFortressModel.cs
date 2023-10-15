using System;

namespace GameCore
{
    public interface IFortressModel
    {
        void Damage();
        event Action OnFortressDestroy;
    }
}