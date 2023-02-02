using System;
using UnityEngine;
using Zenject;
using UniRx;
using Multiplayer.Health.data;
using Multiplayer.Spawn.data;
using Multiplayer.PlayerState.data;
using Multiplayer.PlayerInput.presentation.InputRestriction;
public class SceneSubscriptionsHandler: MonoBehaviour {
[Inject] private PlayerDeathEventHandler m_playerdeatheventhandler;
[Inject] private RestoreHealthOnSpawnHandler m_restorehealthonspawnhandler;
[Inject] private ReadyAfterDeathHandler m_readyafterdeathhandler;
[Inject] private PlayerStateSelfUpdateRepository m_playerstateselfupdaterepository;
[Inject] private PlayerInputStateNavigator m_playerinputstatenavigator;

    private void Awake() {
    m_playerstateselfupdaterepository.HandleUpdates().AddTo(this);
m_playerinputstatenavigator.HandleInputState().AddTo(this);

    }
}
