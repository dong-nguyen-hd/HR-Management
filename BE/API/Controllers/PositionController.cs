using AutoMapper;
using Business.Communication;
using Business.Domain.Models;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Position;
using Business.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

namespace API.Controllers
{
    [Route("api/v1/position")]
    public class PositionController : DongNguyenController<PositionResource, CreatePositionResource, UpdatePositionResource, Position>
    {
        #region Constructor
        public PositionController(IPositionService positionService,
            IMapper mapper,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(positionService, mapper, responseMessage)
        {
        }
        #endregion

        #region Action
        [HttpGet]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PositionResource>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PositionResource>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PositionResource>>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> GetAllAsync()
        {
            Log.Information($"{User.Identity?.Name}: get all position data.");

            return await base.GetAllAsync();
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<PositionResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<PositionResource>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<PositionResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> GetByIdAsync(int id)
        {
            Log.Information($"{User.Identity?.Name}: get a position with Id is {id}.");

            return await base.GetByIdAsync(id);
        }

        [HttpPost]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<PositionResource>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<PositionResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> CreateAsync([FromBody] CreatePositionResource resource)
        {
            Log.Information($"{User.Identity?.Name}: create a position.");

            return await base.CreateAsync(resource);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<PositionResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<PositionResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdatePositionResource resource)
        {
            Log.Information($"{User.Identity?.Name}: update a position with Id is {id}.");

            return await base.UpdateAsync(id, resource);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<PositionResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<PositionResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            Log.Information($"{User.Identity?.Name}: delete a position with Id is {id}.");

            return await base.DeleteAsync(id);
        }
        #endregion
    }
}
