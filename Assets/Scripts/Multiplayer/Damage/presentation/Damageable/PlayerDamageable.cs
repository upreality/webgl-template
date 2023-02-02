using Mirror;
using Multiplayer.Health.domain;
using UnityEngine;
using Zenject;

namespace Multiplayer.Damage.presentation.Damageable
{
    public class PlayerDamageable : NetworkBehaviour, IDamageable
    {
        [Inject] private DecreaseHealthUseCase decreaseHealthUseCase;
        [SerializeField] private string playerId;

        public void TakeDamage(int damage) => RPC_TakeDamage(damage);

        [ClientRpc]
        private void RPC_TakeDamage(int damage)
        {
            if (!isOwned) return;
            decreaseHealthUseCase.DecreaseHealth(playerId, damage);
        }
    }
}