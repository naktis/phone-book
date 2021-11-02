using Api.RequestProcessors.TokenExtractors;
using Api.RequestProcessors.Validators.Interfaces;
using Business.Dto.Requests;
using Business.Dto.Results;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IEntryService _entryService;
        private readonly IKeyValidator _keyValidator;
        private readonly IUserValidator _userValidator;
        private readonly IClaimExtractor _extractor;

        public UserController(ILogger<UserController> logger, IUserService userService,
            IKeyValidator keyValidator, IClaimExtractor extractor,
            IEntryService entryService, IUserValidator userValidator)
        {
            _logger = logger;
            _userService = userService;
            _keyValidator = keyValidator;
            _extractor = extractor;
            _entryService = entryService;
            _userValidator = userValidator;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<LoginResultDto>> Login([FromBody] LoginRequestDto request)
        {
            if (!_userValidator.ValidateLogin(request))
                return BadRequest();

            if (!await _userService.UsernameMatchesPass(request)) {
                _logger.LogInformation($"Log in denied for email address {request.Email}");
                return NotFound();
            }

            var result = await _userService.Authenticate(request);
            _logger.LogInformation($"Log in accepted for user with id = {result.UserId}");
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("Create", Name = nameof(PostUser))]
        public async Task<ActionResult<UserDetailedResultDto>> PostUser([FromBody] UserRequestDto request)
        {
            if (!_userValidator.Validate(request) ||
                await _userService.CredentialsExist(request))
                return BadRequest();

            var user = await _userService.Create(request);
            _logger.LogInformation($"User with id = {user.UserId} has been added");
            return CreatedAtRoute(nameof(PostUser), user);
        }
        
        [Authorize]
        [HttpGet("{entryKey}")]
        public async Task<ActionResult<IEnumerable<UserDetailedResultDto>>> GetUsersOfSharedEntry(int entryKey)
        {
            if (!_keyValidator.Validate(entryKey))
                return BadRequest();

            if (!await _entryService.KeyExists(entryKey))
                return NotFound();

            var ownerKey = _extractor.GetUserId(HttpContext.User.Identity as ClaimsIdentity);

            if (!await _entryService.UserIsOwner(entryKey, ownerKey))
                return BadRequest();

            return Ok(_userService.GetUsersOfSharedEntry(entryKey, ownerKey));
        }

        [Authorize]
        [HttpDelete()]
        public async Task<ActionResult> DeleteEntry()
        {
            var userKey = _extractor.GetUserId(HttpContext.User.Identity as ClaimsIdentity);

            await _userService.Delete(userKey);

            _logger.LogInformation($"User with id = {userKey} has been deleted");
            return Ok();
        }
    }
}
