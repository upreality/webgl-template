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
            Container.Bind<IMatchStateRepository>().To<MatchStateDefaultRepository>().AsSingle();//Domain
            Container.Bind<MatchStateUpdatesUseCase>().ToSelf().AsSingle();
            Container.Bind<NextMatchStateUseCase>().ToSelf().AsSingle();
            Container.Bind<MatchStateTimerStateFlowUseCase>().ToSelf().AsSingle();
            Container.Bind<StartMatchStateUseCase>().ToSelf().AsSingle();
        }
    }
}