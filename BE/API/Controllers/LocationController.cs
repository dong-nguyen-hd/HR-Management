using Business.Communication;
using Business.Domain.Models;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Location;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/v1/location")]
    public class LocationController : DongNguyenController<LocationResource, CreateLocationResource, UpdateLocationResource, Location>
    {
        #region Constructor
        public LocationController(ILocationService locationService,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(locationService, responseMessage)
        {
        }
        #endregion

        #region Action
        [HttpGet]
        [Authorize(Roles = "viewer, editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<LocationResource>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<LocationResource>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<LocationResource>>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> GetAllAsync()
        {
            Log.Information($"{User.Identity?.Name}: get all location data.");

            return await base.GetAllAsync();
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "viewer, editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<LocationResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<LocationResource>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<LocationResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> GetByIdAsync(int id)
        {
            Log.Information($"{User.Identity?.Name}: get a location with Id is {id}.");

            return await base.GetByIdAsync(id);
        }

        [HttpPost]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<LocationResource>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<LocationResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> CreateAsync([FromBody] CreateLocationResource resource)
        {
            Log.Information($"{User.Identity?.Name}: create a location.");

            return await base.CreateAsync(resource);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<LocationResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<LocationResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateLocationResource resource)
        {
            Log.Information($"{User.Identity?.Name}: update a location with Id is {id}.");

            return await base.UpdateAsync(id, resource);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BaseResponse<LocationResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<LocationResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            Log.Information($"{User.Identity?.Name}: delete a location with Id is {id}.");

            return await base.DeleteAsync(id);
        }
        #endregion
    }
}
