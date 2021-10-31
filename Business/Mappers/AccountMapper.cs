using Business.Dto;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mappers
{

    public class AccountMapper : IAccountMapper
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
                Id = user.UserId,
                Username = user.Username,
                Email = user.Email
            };
        }
    }
}
