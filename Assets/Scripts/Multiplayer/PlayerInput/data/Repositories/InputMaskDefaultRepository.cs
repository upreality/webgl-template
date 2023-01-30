using System.Collections.Generic;
using Multiplayer.PlayerInput.domain.model;
using Multiplayer.PlayerInput.domain.model.InputMask;
using Multiplayer.PlayerInput.domain.repositories;

namespace Multiplayer.PlayerInput.data.Repositories
{
    public class InputMaskDefaultRepository: IInputMaskRepository
    {
        private Dictionary<PlayerInputState, IInputMask> masksMap = new()
        {
            { PlayerInputState.Disabled, new InclusiveInputMask() },
            { PlayerInputState.Full, new ExclusiveInputMask() },
        };

        public IInputMask GetInputMask(PlayerInputState state)
        {
            return masksMap[state];
        }
    }
}