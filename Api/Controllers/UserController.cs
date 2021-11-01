using Business.Dto.Requests;
using Business.Dto.Results;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult<LoginResultDto> Login([FromBody]LoginRequestDto request)
        {
            // TODO: validate

            if (!_userService.Exists(request)) {
                _logger.LogInformation($"Login denied for email address {request.Email}");
                return NotFound();
            }

            var result = _userService.Authenticate(request);
            _logger.LogInformation($"Login accepted for email address {request.Email}");
            return Ok(result);
        }

        /*[Authorize] TODO
        public ActionResult Logout()
        {
            return NotFound();
        }*/

        [HttpPost("Create", Name = nameof(PostUser))]
        public async Task<ActionResult<UserDetailedResultDto>> PostUser([FromBody]UserRequestDto request)
        {
            // TODO: validate

            var user = await _userService.Create(request);
            _logger.LogInformation($"User with id {user.UserId} has been added");
            return CreatedAtRoute(nameof(PostUser), user);
        }
    }
}
