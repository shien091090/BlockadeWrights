using Zenject;

namespace GameCore
{
    public class GameSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IInputAxisController>().To<InputAxisController>().AsSingle();
            Container.Bind<PlayerModel>().AsSingle();
        }
    }
}