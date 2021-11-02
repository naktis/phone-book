using Business.Dto.Requests;
using Business.Dto.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IUserService
    {
        public Task<bool> UsernameMatchesPass(LoginRequestDto request);
        public Task<bool> CredentialsExist(UserRequestDto request);
        public Task<LoginResultDto> Authenticate(LoginRequestDto request);
        public Task<UserResultDto> Create(UserRequestDto request);
        public Task<bool> KeyExists(int key);
        public IEnumerable<UserDetailedResultDto> GetUsersOfSharedEntry(int entryKey, int ownerKey);
        public Task Delete(int userKey);
        public Task<int> GetUserIdByUsername(string username);
    }
}
