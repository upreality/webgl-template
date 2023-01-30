using Multiplayer.PlayerInput.domain.model;

namespace Multiplayer.PlayerInput.domain.repositories
{
    internal interface IPlayerInputRepository
    {
        public bool GetSelection(out int selection);
        public float GetAxis(PlayerInputAxis axis);
    }
}