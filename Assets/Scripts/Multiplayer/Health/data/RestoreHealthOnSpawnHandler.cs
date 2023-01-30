using System;
using Multiplayer.Health.domain;
using Multiplayer.PlayerState.domain.model;
using Multiplayer.PlayerState.domain.repositories;
using UniRx;
using Utils.ZenjectCodegen;
using Zenject;

namespace Multiplayer.Health.data
{
    [SceneSubscriptionHandler]
    public class RestoreHealthOnSpawnHandler
    {
        [Inject] private IPlayerLifecycleEventRepository lifecycleEventRepository;
        [Inject] private RestoreHealthUseCase restoreHealthUseCase;

        [SceneSubscription]
        private IDisposable HandleRestoreHealth() => lifecycleEventRepository
            .GetLifecycleEvents()
            .Where(lifecycleEvent => lifecycleEvent == PlayerLifecycleEvent.Spawned)
            .Subscribe(_ => restoreHealthUseCase.RestoreHealth());
    }
}