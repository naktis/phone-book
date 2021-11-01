using Api.RequestProcessors.DefaultSetters;
using Api.RequestProcessors.TokenExtractors;
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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private readonly IEntryParamsSetter _defaultSetter;
        private readonly IClaimExtractor _extractor;
        private readonly IEntryService _entryService;
        private readonly ILogger<UserController> _logger;

        public EntryController(IEntryParamsSetter defaultSetter, IClaimExtractor extractor,
            IEntryService entryService, ILogger<UserController> logger)
        {
            _defaultSetter = defaultSetter;
            _extractor = extractor;
            _entryService = entryService;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<EntryDetailedResultDto>> GetEntries([FromQuery]EntryParameters entryParams)
        {
            /* TODO: validacija
             if (!_entryValidator.Validate(entryParams))
                return BadRequest();
            */

            entryParams = _defaultSetter.SetDefaultValues(entryParams);
            var userId = _extractor.GetUserId(HttpContext.User.Identity as ClaimsIdentity);

            return Ok(_entryService.Get(entryParams, userId));
        }


        [HttpPost(Name = nameof(PostEntry))]
        public async Task<ActionResult<EntryDetailedResultDto>> PostEntry([FromBody] EntryRequestDto request)
        {
            // TODO: validate

            var userId = _extractor.GetUserId(HttpContext.User.Identity as ClaimsIdentity);

            var entry = await _entryService.Create(request, userId);
            _logger.LogInformation($"User (id = {userId}) added a new entry (id = {entry.EntryId})");
            return CreatedAtRoute(nameof(PostEntry), entry);
        }
    }
}
