#nullable enable
using HR_Management.Domain.Services;
using HR_Management.Resources;
using HR_Management.Resources.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HR_Management.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenManagementService _tokenManagementService;

        public TokenController(ITokenManagementService tokenManagementService)
        {
            _tokenManagementService = tokenManagementService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 401)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginResource loginResource)
        {
            var result = await _tokenManagementService.GenerateTokensAsync(loginResource, DateTime.Now);

            if (!result.Success)
                return Unauthorized(new ResultResource(result.Message));

            return Ok(result.Resource);
        }
    }
}
