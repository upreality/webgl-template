using System;
using UnityEngine;
using Zenject;
using UniRx;
using Multiplayer.PlayerManagement.presentation;
public class SceneSubscriptionsHandler: MonoBehaviour {
[Inject] private ConnectionEventsDebug m_connectioneventsdebug;

    private void Awake() {
    m_connectioneventsdebug.LogConnectionEvents().AddTo(this);

    }
}
