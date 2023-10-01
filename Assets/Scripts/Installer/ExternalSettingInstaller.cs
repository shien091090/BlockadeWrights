using UnityEngine;
using Zenject;

namespace GameCore
{
    public class ExternalSettingInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private InGameMapScriptableObject inGameMapSetting;
        [SerializeField] private AttackWaveSettingScriptableObject attackWaveSetting;

        public override void InstallBindings()
        {
            Container.Bind<IInGameMapSetting>().FromInstance(inGameMapSetting);
            Container.Bind<IAttackWaveSetting>().FromInstance(attackWaveSetting);
        }
    }
}