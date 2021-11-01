using Business.Dto.Requests;
using Business.Dto.Results;
using Business.Mappers;
using Businesss.Options;
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

        public UserService(PhoneBookContext context, IUserMapper mapper, 
            IOptions<AccountOptions> options)
        {
            _context = context;
            _mapper = mapper;
            _options = options.Value;
        }

        public LoginResultDto Authenticate(LoginRequestDto request)
        {
            var user = GetUser(request);

            if (user != null)
            {
                var userDto = _mapper.EntityToLoginDto(user);
                userDto.Token = GenerateJwtToken(userDto);
                return userDto;
            }

            return null;
        }

        public bool UsernameMatchesPass(LoginRequestDto request)
        {
            return GetUser(request) != null;
        }

        public bool CredentialsExist(UserRequestDto request)
        {
            var username = _context.Users.FirstOrDefault(u => u.Username == request.Username);
            var password = _context.Users.FirstOrDefault(u => u.Password == request.Password);

            return (username != null || password != null) ;
        }

        public async Task<UserResultDto> Create(UserRequestDto request)
        {
            // TODO: encrypt password ig? or do it in front?
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

        private User GetUser(LoginRequestDto request)
        {
            return _context.Users.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);
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
