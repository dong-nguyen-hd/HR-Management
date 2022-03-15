using AutoMapper;
using Business.Communication;
using Business.Domain.Models;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Group;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/v1/group")]
    public class GroupController : DongNguyenController<GroupResource, CreateGroupResource, UpdateGroupResource, Group>
    {
        #region Constructor
        public GroupController(IGroupService groupService,
            IMapper mapper,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(groupService, mapper, responseMessage)
        {
        }
        #endregion

        #region Action
        [HttpPost]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<GroupResource>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<GroupResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> CreateAsync([FromBody] CreateGroupResource resource)
        {
            Log.Information($"{User.Identity?.Name}: create a group.");

            return await base.CreateAsync(resource);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<GroupResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GroupResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateGroupResource resource)
        {
            Log.Information($"{User.Identity?.Name}: update a group with Id is {id}.");

            return await base.UpdateAsync(id, resource);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BaseResponse<GroupResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<GroupResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            Log.Information($"{User.Identity?.Name}: delete a group with Id is {id}.");

            return await base.DeleteAsync(id);
        }
        #endregion
    }
}
