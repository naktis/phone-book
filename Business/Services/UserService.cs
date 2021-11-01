using Business.Dto.Requests;
using Business.Dto.Results;
using Business.Mappers;
using Businesss.Options;
using Data.Context;
using Data.Models;
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

        public bool Exists(LoginRequestDto request)
        {
            return GetUser(request) != null;
        }
        public async Task<UserResultDto> Create(UserRequestDto request)
        {
            // TODO: encrypt password ig? or do it in front?
            var user = _mapper.DtoToEntity(request);

            var createdUser = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return _mapper.EntityToDetailedDto(createdUser.Entity);
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
