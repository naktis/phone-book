using Business.Dto;
using Data.Models;

namespace Business.Mappers
{
    public interface IAccountMapper
    {
        public UserResultDto EntityToDto(User user);
        public User DtoToEntity(UserRequestDto user);
    }
}
