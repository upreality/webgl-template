using System;
using Multiplayer.Health.domain;
using Multiplayer.PlayerState.domain.model;
using Multiplayer.PlayerState.domain.repositories;
using UniRx;
using Utils.ZenjectCodegen;
using Zenject;

namespace Multiplayer.Health.data
{
    // [SceneSubscriptionHandler]
    public class PlayerDeathEventHandler
    {
        [Inject] private PlayerDeathEventUseCase deathEventUseCase;
        [Inject] private IPlayerLifecycleEventRepository lifecycleEventRepository;

        // [SceneSubscription]
        private IDisposable HandleDeathEvent() => deathEventUseCase
            .GetDeathEventFlow()
            .Subscribe(_ =>
                lifecycleEventRepository.SendLifecycleEvent(PlayerLifecycleEvent.Died)
            );
    }
}