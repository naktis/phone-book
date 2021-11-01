namespace Api.RequestProcessors.Validators.Interfaces
{
    public interface ISharedValidator
    {
        public bool TextLengthValid(string text, int min, int max);
    }
}
