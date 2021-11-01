using Api.RequestProcessors.DefaultSetters;
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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private readonly IEntryParamsSetter _defaultSetter;
        private readonly IClaimExtractor _extractor;
        private readonly IEntryService _entryService;
        private readonly IUserService _userService;
        private readonly ILogger<EntryController> _logger;
        private readonly IKeyValidator _keyValidator;
        private readonly IEntryParamsValidator _paramsValidator;
        private readonly IEntryValidator _entryValidator;

        public EntryController(IEntryParamsSetter defaultSetter, 
            IClaimExtractor extractor, IEntryService entryService, 
            ILogger<EntryController> logger, IKeyValidator keyValidator, 
            IEntryParamsValidator paramsValidator, IEntryValidator entryValidator,
            IUserService userService)
        {
            _defaultSetter = defaultSetter;
            _extractor = extractor;
            _entryService = entryService;
            _logger = logger;
            _keyValidator = keyValidator;
            _paramsValidator = paramsValidator;
            _entryValidator = entryValidator;
            _userService = userService;
        }

        [HttpGet("{key}")]
        public async Task<ActionResult<EntryDetailedResultDto>> GetEntry(int key)
        {
            var userId = _extractor.GetUserId(HttpContext.User.Identity as ClaimsIdentity);

            if (!_keyValidator.Validate(key))
                return BadRequest();

            if (!await _entryService.KeyExists(key))
                return NotFound();

            if (!await _entryService.UserHasAccess(key, userId))
                return BadRequest();

            return Ok(_entryService.Get(key));
        }

        [HttpGet]
        public ActionResult<IEnumerable<EntryDetailedResultDto>> GetEntries([FromQuery]EntryParameters entryParams)
        {
            if (!_paramsValidator.Validate(entryParams))
                return BadRequest();

            entryParams = _defaultSetter.SetDefaultValues(entryParams);
            var userId = _extractor.GetUserId(HttpContext.User.Identity as ClaimsIdentity);

            return Ok(_entryService.Get(entryParams, userId));
        }


        [HttpPost(Name = nameof(PostEntry))]
        public async Task<ActionResult<EntryResultDto>> PostEntry([FromBody]EntryRequestDto request)
        {
            var userId = _extractor.GetUserId(HttpContext.User.Identity as ClaimsIdentity);

            if (!_entryValidator.Validate(request) || await _entryService.Exists(request, userId))
                return BadRequest();

            var entry = await _entryService.Create(request, userId);

            _logger.LogInformation($"User (id = {userId}) has added a new entry (id = {entry.EntryId})");
            return CreatedAtRoute(nameof(PostEntry), entry);
        }

        [HttpPost("Share", Name = nameof(ShareEntry))]
        public async Task<ActionResult> ShareEntry(int entryKey, int receiverKey)
        {
            if (!_keyValidator.Validate(entryKey) || !_keyValidator.Validate(receiverKey))
                return BadRequest();

            if (!await _entryService.KeyExists(entryKey) || !await _userService.KeyExists(receiverKey))
                return NotFound();

            var senderKey = _extractor.GetUserId(HttpContext.User.Identity as ClaimsIdentity);

            if (!await _entryService.UserIsOwner(entryKey, senderKey) ||
                await _entryService.UserHasAccess(entryKey, receiverKey))
                return BadRequest();

            await _entryService.Share(entryKey, receiverKey);

            _logger.LogInformation($"User (id = {senderKey}) has shared an entry (id = {entryKey})" +
                $"with another user (id = {receiverKey})");
            return CreatedAtRoute(nameof(ShareEntry), new { id = entryKey });
        }

        [HttpDelete("Share/{entryKey}/{receiverKey}")]
        public async Task<ActionResult> CancelShare(int entryKey, int receiverKey)
        {
            if (!_keyValidator.Validate(entryKey) || !_keyValidator.Validate(receiverKey))
                return BadRequest();

            if (!await _entryService.KeyExists(entryKey) || !await _userService.KeyExists(receiverKey))
                return NotFound();

            var senderKey = _extractor.GetUserId(HttpContext.User.Identity as ClaimsIdentity);

            if (!await _entryService.UserIsOwner(entryKey, senderKey) ||
                !await _entryService.UserHasAccess(entryKey, receiverKey))
                return BadRequest();

            await _entryService.CancelShare(entryKey, receiverKey);

            _logger.LogInformation($"User (id = {senderKey}) has cancelled sharing entry (id = {entryKey})" +
                $"with another user (id = {receiverKey})");
            return Ok();
        }

        [HttpPut("{key}")]
        public async Task<ActionResult> UpdateEntry([FromRoute] int key, [FromBody] EntryRequestDto request)
        {
            if (!_keyValidator.Validate(key))
                return BadRequest();

            if (!await _entryService.KeyExists(key))
                return NotFound();

            var userId = _extractor.GetUserId(HttpContext.User.Identity as ClaimsIdentity);

            if (!_entryValidator.Validate(request) ||
                !await _entryService.UserHasAccess(key, userId) ||
                await _entryService.Exists(request, userId))
                return BadRequest();

            await _entryService.Update(request, key, userId);

            _logger.LogInformation($"User (id = {userId}) has updated an entry (id = {key})");
            return Ok();
        }

        [HttpDelete("{key}")]
        public async Task<ActionResult> DeleteEntry(int key)
        {
            if (!_keyValidator.Validate(key))
                return BadRequest();

            if (!await _entryService.KeyExists(key))
                return NotFound();

            var userId = _extractor.GetUserId(HttpContext.User.Identity as ClaimsIdentity);

            if (!await _entryService.UserHasAccess(key, userId))
                return BadRequest();

            await _entryService.Delete(key);

            _logger.LogInformation($"Entry with id={key} has been deleted");
            return Ok();
        }
    }
}
