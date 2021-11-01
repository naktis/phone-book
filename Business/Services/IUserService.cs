using Business.Dto.Requests;
using Business.Dto.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IUserService
    {
        public bool UsernameMatchesPass(LoginRequestDto request);
        public bool CredentialsExist(UserRequestDto request);
        public LoginResultDto Authenticate(LoginRequestDto request);
        public Task<UserResultDto> Create(UserRequestDto request);
        public Task<bool> KeyExists(int key);
        IEnumerable<UserDetailedResultDto> GetUsersOfSharedEntry(int entryKey, int ownerKey);
        public Task Delete(int userKey);
    }
}
