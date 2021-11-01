using Business.Dto.Requests;

namespace Api.RequestProcessors.Validators.Interfaces
{
    public interface IUserValidator : IValidator<UserRequestDto> 
    {
        public bool ValidateLogin(LoginRequestDto request);
    }
}
