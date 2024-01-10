using Jiwebapi.Catalog.Application.Contracts;
using Jiwebapi.Catalog.Application.Contracts.Cache;
using Jiwebapi.Catalog.Application.Contracts.Identity;
using Jiwebapi.Catalog.Application.Models.Authentication;
using Jiwebapi.Catalog.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jiwebapi.Catalog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ISimpleStorageService _simpleStorageService;
        private readonly IObjectStorageService _objectStorageService;
        private readonly ILoggedInUserService _loggedInUserService;

        public AccountController(IAuthenticationService authenticationService, ISimpleStorageService simpleStorageService, IObjectStorageService objectStorageService, ILoggedInUserService loggedInUserService)
        {
            _authenticationService = authenticationService;
            _simpleStorageService = simpleStorageService;
            _objectStorageService = objectStorageService;
            _loggedInUserService = loggedInUserService;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            var result = await _authenticationService.AuthenticateAsync(request);
            var cacheId = await _simpleStorageService.Set($"{result.Email}|{result.Token}");
            result.CacheId = cacheId;
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> RegisterAsync(RegistrationRequest request)
        {
            var result = await _authenticationService.RegisterAsync(request);
            await _objectStorageService.CreateItem(new StorageObject
            {
                Id = result.UserId,
                Value = result.Email,
            });
            return Ok(result);
        }

        [HttpPost("check")]
        public async Task<ActionResult<CheckResponse>> CheckAsync(CheckRequest? request)
        {
            if (request == null || string.IsNullOrEmpty(request.CacheId))
            {
                return Unauthorized();
            }

            var cacheData = await _simpleStorageService.Get(request.CacheId);
            if (cacheData == null || string.IsNullOrEmpty(cacheData.Value))
            {
                return Unauthorized();
            }

            var dataArr = cacheData.Value.Split('|');
            if (dataArr.Length == 2)
            {
                return Ok(new CheckResponse
                {
                    Email = dataArr[0],
                    Token = dataArr[1],
                });
            }

            return Unauthorized();
        }

        [HttpPost("refresh")]
        [Authorize]
        public async Task<ActionResult<RefreshResponse>> RefreshAsync(RefreshRequest request)
        {
            var userFromContext = _loggedInUserService.UserId;

            if (userFromContext != request.UserId)
            {
                return Unauthorized();
            }       

            var result = await _authenticationService.RefreshAsync(request);
            return Ok(result);
        }
    }
}
