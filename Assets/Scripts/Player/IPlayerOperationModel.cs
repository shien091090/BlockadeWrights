using System;

namespace GameCore
{
    public interface IPlayerOperationModel
    {
        bool CreateBuilding(IInGameMapCell targetMapCell);
        event Action<IInGameMapCell> OnCreateBuilding;
    }
}