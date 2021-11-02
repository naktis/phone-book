using Api.RequestProcessors.Validators.Interfaces;
using Business.Dto.Requests;
using System.Linq;

namespace Api.RequestProcessors.Validators
{
    public class UserValidator : IUserValidator
    {
        public readonly int MinEmailLength = 3;
        public readonly int MaxEmailLength = 30;
        public readonly int MinPassLength = 8;
        public readonly int MaxPassLength = 30;
        public readonly ISharedValidator _sharedValidator;

        public UserValidator(ISharedValidator sharedValidator)
        {
            _sharedValidator = sharedValidator;
        }

        public bool ValidateLogin(LoginRequestDto request)
        {
            return ValidateEmail(request.Email) && ValidatePassword(request.Password);
        }

        public bool Validate(UserRequestDto request)
        {
            return ValidateEmail(request.Email) && 
                   ValidatePassword(request.Password) &&
                   ValidateUsername(request.Username);
        }

        public bool ValidateUsername(string username)
        {
            return username.All(u => char.IsLetterOrDigit(u));
        }

        private bool ValidatePassword(string password)
        {
            return _sharedValidator.TextLengthValid(password, MaxPassLength, MinPassLength);
        }

        private bool ValidateEmail(string email)
        {
            return _sharedValidator.TextLengthValid(email, MaxEmailLength, MinEmailLength) &&
                   email.Contains('@') &&
                   email.Contains('.');
        }
    }
}
