using System;
using UniRx;
using Utils.WebSocketClient.domain.model;

namespace Utils.WebSocketClient.domain
{
    public interface IWSConnectionRepository
    {
        IObservable<WSConnectionState> ConnectionState { get; }
        IObservable<Unit> ConnectBaseAuth(WSAuthData authData);
        IObservable<Unit> Connect();
    }
}