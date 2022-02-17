using Business.Communication;
using Business.Data;
using Business.Domain.Models;
using Business.Domain.Services;
using Business.Extensions;
using Business.Resources;
using Business.Resources.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/v1/account")]
    public class AccountController : DongNguyenController<AccountResource, CreateAccountResource, UpdateAccountResource, Account>
    {
        #region Property
        private readonly IAccountService _accountService;
        private readonly IImageService _imageService;
        #endregion

        #region Constructor
        public AccountController(IAccountService accountService,
            IImageService imageService,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(accountService, responseMessage)
        {
            this._accountService = accountService;
            this._imageService = imageService;
        }
        #endregion

        #region Action
        [HttpPut("image/{id:int}")]
        [Authorize(Roles = "viewer, editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<AccountResource>), 200)]
        [ProducesResponseType(typeof(BaseResponse<AccountResource>), 400)]
        public async Task<IActionResult> SaveImageAsync(int id, [FromForm] IFormFile image)
        {
            Log.Information($"{User.Identity?.Name}: save image for Id is {id}.");

            var identifier = (User.Identity as ClaimsIdentity).FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

            if (!identifier.Equals(id.ToString()))
                return BadRequest(new BaseResponse<AccountResource>(ResponseMessage.Values["Account_Not_Permitted"]));

            var filePath = Path.GetTempFileName();

            var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);
            var result = await _imageService.SaveImageAccountAsync(id, stream);
            stream.Dispose();

            // Clean temp-file
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<AccountResource>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<AccountResource>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<AccountResource>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListPaginationAsync([FromQuery] int page, [FromQuery] int pageSize)
        {
            Log.Information($"{User.Identity?.Name}: get account data.");

            QueryResource pagintation = new QueryResource(page, pageSize);

            var result = await _accountService.ListPaginationAsync(pagintation);

            if (!result.Success)
                return BadRequest(result);

            if (result.Resource is null)
                return NoContent();

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<AccountResource>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<AccountResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> CreateAsync([FromBody] CreateAccountResource resource)
        {
            Log.Information($"{User.Identity?.Name}: create account is {resource.UserName}.");

            if (resource.Role == (int)eRole.Admin)
            {
                if (!User.IsInRole(eRole.Admin.ToDescriptionString()))
                    return BadRequest(new BaseResponse<AccountResource>(ResponseMessage.Values["Account_Not_Permitted"]));
            }

            var result = await _accountService.InsertAsync(resource);

            if (!result.Success)
                return BadRequest(result);

            return StatusCode(201, result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BaseResponse<AccountResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<AccountResource>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<AccountResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> GetByIdAsync(int id)
        {
            Log.Information($"{User.Identity?.Name}: get account data with Id is {id}.");

            return await base.GetByIdAsync(id);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BaseResponse<AccountResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<AccountResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateAccountResource resource)
        {
            Log.Information($"{User.Identity?.Name}: update account with Id is {id}.");

            return await base.UpdateAsync(id, resource);
        }

        [HttpPut("self-update/{id:int}")]
        [Authorize(Roles = "admin, editor, viewer")]
        [ProducesResponseType(typeof(BaseResponse<AccountResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<AccountResource>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SeflUpdateAsync(int id, [FromBody] SelfUpdateAccountResource resource)
        {
            Log.Information($"{User.Identity?.Name}: self-update account with Id is {id}.");

            var identifier = (User.Identity as ClaimsIdentity).FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

            if (!identifier.Equals(id.ToString()))
                return BadRequest(new BaseResponse<AccountResource>(ResponseMessage.Values["Account_Not_Permitted"]));

            var result = await _accountService.SelfUpdateAsync(id, resource);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("change-password/{id:int}")]
        [Authorize(Roles = "admin, editor, viewer")]
        [ProducesResponseType(typeof(BaseResponse<AccountResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<AccountResource>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePasswordAsync(int id, [FromBody] UpdatePasswordAccountResource resource)
        {
            Log.Information($"{User.Identity?.Name}: update account password with Id is {id}.");

            // Check if the id belongs to me
            var identifier = (User.Identity as ClaimsIdentity).FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            if (!identifier.Equals(id.ToString()))
                return BadRequest(new BaseResponse<AccountResource>(ResponseMessage.Values["Account_Not_Permitted"]));

            // Checking duplicate password
            if (resource.OldPassword.Equals(resource.NewPassword))
                return BadRequest(new BaseResponse<AccountResource>(ResponseMessage.Values["Account_Not_Permitted"]));

            var result = await _accountService.UpdatePasswordAsync(id, resource);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BaseResponse<AccountResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<AccountResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            Log.Information($"{User.Identity?.Name}: delete account with Id is {id}.");

            return await base.DeleteAsync(id);
        }

        [HttpPost("delete-range")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BaseResponse<AccountResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<AccountResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> DeleteRangeAsync(List<int> ids)
        {
            Log.Information($"{User.Identity?.Name}: delete range account with Ids is {ids}.");

            if (ids.Count <= 0)
                return BadRequest(new BaseResponse<AccountResource>(false));

            return await base.DeleteRangeAsync(ids);
        }
        #endregion
    }
}
