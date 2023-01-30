using System;
using Multiplayer.PlayerInput.domain.model;
using UniRx;
using UnityEngine;
using Zenject;

namespace Multiplayer.PlayerInput.presentation.InputRestriction
{
    public class ActiveStateRestrictionSource: MonoBehaviour, PlayerInputStateNavigator.IRestrictionSource
    {
        [Inject] private PlayerInputStateNavigator navigator;

        [SerializeField] private PlayerInputState restrictionLevel = PlayerInputState.Disabled;

        private Subject<PlayerInputState> restrictionSubject = new();

        private void Awake() => navigator.AddRestrictionSource(this);

        public IObservable<PlayerInputState> GetInputState() => restrictionSubject;

        private void OnEnable()
        {
            restrictionSubject.OnNext(restrictionLevel);
        }

        private void OnDisable()
        {
            restrictionSubject.OnNext(PlayerInputState.Disabled);
        }

        private void OnDestroy()
        {
            restrictionSubject.OnNext(PlayerInputState.Disabled);
            restrictionSubject.Dispose();
        }
    }
}