namespace Multiplayer.PlayerInput.domain.model.InputMask
{
    public interface IInputMask
    {
        public bool InputAvailable(PlayerInputAxis axis);
        public bool NumericInputAvailable();
    }
}