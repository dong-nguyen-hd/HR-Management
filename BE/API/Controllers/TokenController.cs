using Business.Communication;
using Business.Domain.Services;
using Business.Resources.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/token")]
    public class TokenController : ControllerBase
    {
        #region Property
        private readonly ITokenManagementService _tokenManagementService;
        #endregion

        #region Constructor
        public TokenController(ITokenManagementService tokenManagementService)
        {
            this._tokenManagementService = tokenManagementService;
        }
        #endregion

        #region Action
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(BaseResponse<TokenResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<TokenResource>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginResource resource)
        {
            string userAgent = Request.Headers["User-Agent"].ToString();
            var result = await _tokenManagementService.GenerateTokensAsync(resource, DateTime.UtcNow, userAgent);

            if (result.Success)
                return Ok(result);

            return Unauthorized(result);
        }
        #endregion
    }
}
