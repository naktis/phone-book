namespace Api.RequestProcessors.Validators.Interfaces
{
    public interface IValidator<T>
    {
        public bool Validate(T input);
    }
}
