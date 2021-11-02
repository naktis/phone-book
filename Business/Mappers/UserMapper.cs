using Business.Dto.Requests;
using Business.Dto.Results;
using Data.Models;

namespace Business.Mappers
{
    public class UserMapper : IUserMapper
    {
        public User DtoToEntity(UserRequestDto user)
        {
            return new User
            {
                Username = user.Username,
                Email = user.Email,
                Password = user.Password
            };
        }

        public UserResultDto EntityToDto(User user)
        {
            return new UserResultDto
            {
                UserId = user.UserId
            };
        }

        public UserDetailedResultDto EntityToDetailedDto(User user)
        {
            return new UserDetailedResultDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email
            };
        }

        public LoginResultDto EntityToLoginDto(User user)
        {
            return new LoginResultDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email
            };
        }
    }
}
