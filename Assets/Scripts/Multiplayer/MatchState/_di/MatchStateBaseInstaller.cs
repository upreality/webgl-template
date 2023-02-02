using Multiplayer.MatchState.data;
using Multiplayer.MatchState.domain;
using Multiplayer.MatchState.domain.repositories;
using Multiplayer.MatchState.presentation.SyncHandler;
using UnityEngine;
using Zenject;

namespace Multiplayer.MatchState._di
{
    [CreateAssetMenu(fileName = "MatchStateBaseInstaller", menuName = "Installers/MatchStateBaseInstaller", order = 0)]
    internal class MatchStateBaseInstaller : ScriptableObjectInstaller
    {
        // ReSharper disable once InconsistentNaming
        [SerializeField] private MatchStateDurationScriptableObjectRepository stateSoRepository;
        public override void InstallBindings()
        {
            //Data
            Container.Bind<IMatchStateDurationRepository>().FromInstance(stateSoRepository).AsSingle();
            //Domain
            Container.Bind<NextMatchStateUseCase>().ToSelf().AsSingle();
            //Sync
            Container.BindInterfacesAndSelfTo<MatchStateSyncHandler>().AsSingle();
        }
    }
}