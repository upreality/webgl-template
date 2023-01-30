using System.Collections.Generic;

namespace Multiplayer.PlayerInput.domain.model.InputMask
{
    public class ExclusiveInputMask: IInputMask
    {
        private List<PlayerInputAxis> axisList;
        private bool numeric;

        public ExclusiveInputMask(
            List<PlayerInputAxis> axisList,
            bool excludeNumeric = true
        )
        {
            this.axisList = axisList;
            numeric = excludeNumeric;
        }
        
        public ExclusiveInputMask()
        {
            axisList = new List<PlayerInputAxis>();
            numeric = false;
        }

        public bool InputAvailable(PlayerInputAxis axis) => !axisList.Contains(axis);

        public bool NumericInputAvailable() => !numeric;
    }
}