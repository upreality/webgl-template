using System;
using Multiplayer.PlayerManagement.domain;
using UniRx;
using UnityEngine;
using Utils.ZenjectCodegen;
using Zenject;

namespace Multiplayer.PlayerManagement.presentation
{
    [SceneSubscriptionHandler]
    public class ConnectionEventsDebug
    {
        [Inject] private IPlayerConnectionEventsRepository connectionEventsRepository;

        [SceneSubscription]
        public IDisposable LogConnectionEvents() => connectionEventsRepository
            .GetConnectionEvents()
            .Subscribe(LogEvent);

        private static void LogEvent(PlayerConnectionEvent playerConnectionEvent)
        {
            var state = playerConnectionEvent.Connected ? "Connected" : "Disconnected";
            Debug.Log($"Player {playerConnectionEvent.PlayerId} {state}!");
        }
    }
}