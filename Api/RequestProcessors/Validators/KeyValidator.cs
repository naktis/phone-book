using Api.RequestProcessors.Validators.Interfaces;

namespace Api.RequestProcessors.Validators
{
    public class KeyValidator : IKeyValidator
    {
        public bool Validate(int key)
        {
            return key >= 0;
        }
    }
}
