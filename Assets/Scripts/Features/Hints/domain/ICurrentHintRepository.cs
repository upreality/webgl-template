namespace Features.Hints.domain
{
    public interface ICurrentHintRepository
    {
        void SetHintText(string text);
        string GetHintText();
    }
}
