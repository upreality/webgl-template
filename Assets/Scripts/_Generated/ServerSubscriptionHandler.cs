using System;
using UnityEngine;
using Zenject;
using Mirror;
using UniRx;
using Multiplayer.MatchState.presentation.SyncHandler;
public class ServerSubscriptionHandler: NetworkBehaviour {
private CompositeDisposable serverSubscriptions = new();
[Inject] private MatchStateSyncHandler m_matchstatesynchandler;

    public override void OnStartServer() {
        var m_matchstatesynchandlerDisposable = m_matchstatesynchandler.HandleMatchState();
serverSubscriptions.Add(m_matchstatesynchandlerDisposable);

    }
    public override void OnStopServer() {
        if(!serverSubscriptions.IsDisposed)
        serverSubscriptions.Dispose();
        serverSubscriptions = new CompositeDisposable();
    }
}
