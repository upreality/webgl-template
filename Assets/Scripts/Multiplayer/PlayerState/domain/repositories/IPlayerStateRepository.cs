using System;
using Multiplayer.PlayerState.domain.model;

namespace Multiplayer.PlayerState.domain.repositories
{
    public interface IPlayerStateRepository
    {
        public IObservable<PlayerStates> GetPlayerStateFlow();
        public PlayerStates GetPlayerState();
    }
}