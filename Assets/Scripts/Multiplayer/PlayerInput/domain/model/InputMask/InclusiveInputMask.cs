using System.Collections.Generic;

namespace Multiplayer.PlayerInput.domain.model.InputMask
{
    public class InclusiveInputMask : IInputMask
    {
        private List<PlayerInputAxis> axisList;
        private bool numeric;

        public InclusiveInputMask(
            List<PlayerInputAxis> axisList,
            bool numeric = false
        )
        {
            this.axisList = axisList;
            this.numeric = numeric;
        }

        public InclusiveInputMask()
        {
            axisList = new List<PlayerInputAxis>();
            numeric = false;
        }

        public bool InputAvailable(PlayerInputAxis axis) => axisList.Contains(axis);

        public bool NumericInputAvailable() => numeric;
    }
}