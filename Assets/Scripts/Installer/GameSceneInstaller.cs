using Zenject;

namespace GameCore
{
    public class GameSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInputAxisController>().To<InputAxisController>().AsSingle();
            Container.Bind<IInputKeyController>().To<InputKeyController>().AsSingle();
            Container.Bind<IPlayerOperationModel>().To<PlayerOperationModel>().AsSingle();
            Container.Bind<IInGameMapModel>().To<InGameMapModel>().AsSingle();
            Container.Bind<PlayerModel>().AsSingle();
            Container.Bind<MonsterSpawner>().AsSingle();
        }
    }
}