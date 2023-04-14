using System;
using ModestTree;
using UniRx;
using UnityEngine;

namespace z_MultiplayerVanilla.Scripts
{
    public class VanillaPlayerId : MonoBehaviour, IPlayerIdProvider
    {
        private readonly BehaviorSubject<string> playerIdSubject = new(string.Empty);

        public IObservable<string> PlayerIdFlow => playerIdSubject
            .Where(id => !id.IsEmpty())
            .DistinctUntilChanged();

        public void Set(string id) => playerIdSubject.OnNext(id);
    }

    public interface IPlayerIdProvider
    {
        IObservable<string> PlayerIdFlow { get; }
    }
}