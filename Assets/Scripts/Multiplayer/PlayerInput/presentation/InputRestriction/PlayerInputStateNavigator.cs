using System;
using System.Collections.Generic;
using System.Linq;
using Multiplayer.PlayerInput.domain.model;
using Multiplayer.PlayerInput.domain.repositories;
using UniRx;
using Utils.ZenjectCodegen;
using Zenject;

namespace Multiplayer.PlayerInput.presentation.InputRestriction
{
    // [SceneSubscriptionHandler]
    public class PlayerInputStateNavigator
    {
        [Inject] private IInputStateRepository inputStateRepository;
        private BehaviorSubject<List<IRestrictionSource>> sourcesFlow = new(new List<IRestrictionSource>());

        // [SceneSubscription]
        public IDisposable HandleInputState() => sourcesFlow.Select(list =>
            list
                .Select(source => source.GetInputState())
                .CombineLatest()
                .Select(latest => latest.Max(playerInputState => (int) playerInputState))
                .Select(priority => (PlayerInputState) priority)
        ).Switch().Subscribe();

        public void AddRestrictionSource(IRestrictionSource source)
        {
            var list = sourcesFlow.Value;
            if (!list.Contains(source))
                list.Add(source);
            sourcesFlow.OnNext(list);
        }

        public interface IRestrictionSource
        {
            IObservable<PlayerInputState> GetInputState();
        }
    }
}