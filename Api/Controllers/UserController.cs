using Business.Dto;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult<UserResultDto> Login([FromBody]LoginRequestDto request)
        {
            // TODO: validate

            if (!_userService.Exists(request)) {
                _logger.LogInformation($"Login denied for address {request.Email}");
                return NotFound();
            }

            var result = _userService.Authenticate(request);
            _logger.LogInformation($"Login accepted for address {request.Email}");
            return Ok(result);
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<ActionResult<int>> PostUser([FromBody]UserRequestDto request)
        {
            // TODO: validate

            var user = await _userService.Create(request);
            _logger.LogInformation($"User with id {user.Id} has been added");
            return Ok(user.Id);
        }
    }
}
