using UnityEngine;
using Zenject;

namespace GameCore
{
    public class ExternalSettingInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private InGameMapScriptableObject inGameMapSetting;

        public override void InstallBindings()
        {
            Container.Bind<IInGameMapSetting>().FromInstance(inGameMapSetting);
        }
    }
}