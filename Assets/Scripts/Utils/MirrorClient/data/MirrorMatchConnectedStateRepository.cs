using System;
using JetBrains.Annotations;
using Mirror;
using UniRx;
using Utils.MirrorClient.domain;

namespace Utils.MirrorClient.data
{
    public class MirrorMatchConnectedStateRepository: IMatchConnectedStateRepository
    {
        [CanBeNull] private BehaviorSubject<MatchConnectedState> stateSubject;

        public IObservable<MatchConnectedState> GetConnectedState() => stateSubject ??= CreateStateSubject();

        private static BehaviorSubject<MatchConnectedState> CreateStateSubject()
        {
            var initialState = NetworkClient.isConnected
                ? MatchConnectedState.Connected
                : MatchConnectedState.Disconnected;

            var subject = new BehaviorSubject<MatchConnectedState>(initialState);
            NetworkClient.OnConnectedEvent += () => subject.OnNext(MatchConnectedState.Connected);
            NetworkClient.OnDisconnectedEvent += () => subject.OnNext(MatchConnectedState.Disconnected);
            return subject;
        }
    }
}