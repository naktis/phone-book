using Business.Dto;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IUserService
    {
        public bool Exists(LoginRequestDto request);
        public UserResultDto Authenticate(LoginRequestDto request);
        Task<UserResultDto> Create(UserRequestDto request);
    }
}
