using Business.Dto.Requests;
using Business.Dto.Results;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IUserService
    {
        public bool Exists(LoginRequestDto request);
        public LoginResultDto Authenticate(LoginRequestDto request);
        Task<UserResultDto> Create(UserRequestDto request);
    }
}
