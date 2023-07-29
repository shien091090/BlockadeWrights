using System;

namespace GameCore.Tests.Player
{
    public interface IPlayerOperationModel
    {
        bool CreateBuilding(IInGameMapCell targetMapCell);
        event Action<IInGameMapCell> OnCreateBuilding;
    }
}