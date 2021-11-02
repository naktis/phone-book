using Business.Dto.Requests;
using Business.Dto.Results;
using Business.Mappers;
using Business.Options;
using Data.Context;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly PhoneBookContext _context;
        private readonly IUserMapper _mapper;
        private readonly AccountOptions _options;
        private readonly IPasswordHasher _hasher;

        public UserService(PhoneBookContext context, IUserMapper mapper, 
            IOptions<AccountOptions> options, IPasswordHasher hasher)
        {
            _context = context;
            _mapper = mapper;
            _options = options.Value;
            _hasher = hasher;
        }

        public async Task<LoginResultDto> Authenticate(LoginRequestDto request)
        {
            var user = await GetUserByEmailPassword(request);

            if (user != null)
            {
                var userDto = _mapper.EntityToLoginDto(user);
                userDto.Token = GenerateJwtToken(userDto);
                return userDto;
            }

            return null;
        }

        public async Task<bool> UsernameMatchesPass(LoginRequestDto request)
        {
            return await GetUserByEmailPassword(request) != null;
        }

        public async Task<bool> CredentialsExist(UserRequestDto request)
        {
            var username = await GetUserByUsername(request.Username);
            var email = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            return (username != null || email != null);
        }

        public async Task<UserResultDto> Create(UserRequestDto request)
        {
            request.Password = _hasher.Hash(request.Password);
            var user = _mapper.DtoToEntity(request);

            var createdUser = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return _mapper.EntityToDetailedDto(createdUser.Entity);
        }

        public async Task<bool> KeyExists(int key)
        {
            return await _context.Users.FindAsync(key) != null;
        }

        public IEnumerable<UserDetailedResultDto> GetUsersOfSharedEntry(int entryKey, int ownerKey)
        {
            var users = _context.UserEntries
                .Where(ue => ue.EntryId == entryKey && ue.UserId != ownerKey)
                .Include(ue => ue.User)
                .ToList();

            var userDtos = new List<UserDetailedResultDto>();
            foreach (var u in users)
                userDtos.Add(_mapper.EntityToDetailedDto(u.User));

            return userDtos;
        }

        public async Task Delete(int key)
        {
            var ownedEntries = await _context.Entries
                .Where(e => e.OwnerId == key)
                .Include(e => e.UserEntries)
                .ToListAsync();

            foreach (var e in ownedEntries)
                _context.RemoveRange(e.UserEntries);

            _context.Entries.RemoveRange(ownedEntries);

            var foreignEntries = await _context.UserEntries
                .Where(ue => ue.UserId == key)
                .ToListAsync();

            _context.UserEntries.RemoveRange(foreignEntries);

            var user = await _context.Users.FindAsync(key);
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetUserIdByUsername(string username)
        {
            var user = await GetUserByUsername(username);

            if (user != null)
                return user.UserId;

            return 0;
        }

        private async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        private async Task<User> GetUserByEmailPassword(LoginRequestDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user != null)
            {
                if (_hasher.Check(user.Password, request.Password))
                    return user;
            }
            
            return null;
        }

        private string GenerateJwtToken(UserDetailedResultDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.Secret);

            var claims = new List<Claim>
            {
                new Claim("id", user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(_options.Expires),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}
