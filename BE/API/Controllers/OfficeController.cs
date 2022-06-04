﻿using AutoMapper;
using Business.Communication;
using Business.Domain.Models;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Position;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

namespace API.Controllers
{
    [Route("api/v1/office")]
    public class OfficeController : DongNguyenController<PositionResource, CreatePositionResource, UpdatePositionResource, Position>
    {
        #region Constructor
        public OfficeController(IPositionService officeService,
            IMapper mapper,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(officeService, mapper, responseMessage)
        {
        }
        #endregion

        #region Action
        [HttpGet]
        [Authorize(Roles = "viewer, editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PositionResource>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PositionResource>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PositionResource>>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> GetAllAsync()
        {
            Log.Information($"{User.Identity?.Name}: get all office data.");

            return await base.GetAllAsync();
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "viewer, editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<PositionResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<PositionResource>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<PositionResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> GetByIdAsync(int id)
        {
            Log.Information($"{User.Identity?.Name}: get a office with Id is {id}.");

            return await base.GetByIdAsync(id);
        }

        [HttpPost]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<PositionResource>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<PositionResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> CreateAsync([FromBody] CreatePositionResource resource)
        {
            Log.Information($"{User.Identity?.Name}: create a office.");

            return await base.CreateAsync(resource);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<PositionResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<PositionResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdatePositionResource resource)
        {
            Log.Information($"{User.Identity?.Name}: update a office with Id is {id}.");

            return await base.UpdateAsync(id, resource);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BaseResponse<PositionResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<PositionResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            Log.Information($"{User.Identity?.Name}: delete a office with Id is {id}.");

            return await base.DeleteAsync(id);
        }
        #endregion
    }
}
