using Business.Dto.Requests;
using Business.Dto.Results;
using Data.Models;

namespace Business.Mappers
{
    public interface IUserMapper : IMapper<User, UserRequestDto, UserDetailedResultDto, UserResultDto> 
    {
        public LoginResultDto EntityToLoginDto(User user);
    }
}
