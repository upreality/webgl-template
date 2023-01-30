using System;
using Multiplayer.PlayerState.domain.model;
using Multiplayer.PlayerState.domain.repositories;
using Multiplayer.Spawn.domain;
using UniRx;
using Utils.ZenjectCodegen;
using Zenject;

namespace Multiplayer.Spawn.data
{
    [SceneSubscriptionHandler]
    public class ReadyAfterDeathHandler
    {
        [Inject] private ReadyAfterDeathUseCase readyAfterDeathUseCase;
        [Inject] private IPlayerLifecycleEventRepository lifecycleEventRepository;

        private readonly int readyDelay = 1;

        [SceneSubscription]
        private IDisposable BecomeReadyAfterDeathSubscription() => readyAfterDeathUseCase
            .GetReadyCommandFlow(readyDelay)
            .Subscribe(_ =>
                lifecycleEventRepository.SendLifecycleEvent(PlayerLifecycleEvent.Ready)
            );
    }
}