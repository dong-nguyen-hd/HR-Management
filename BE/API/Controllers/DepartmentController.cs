﻿using AutoMapper;
using Business.Communication;
using Business.Domain.Models;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Department;
using Business.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

namespace API.Controllers
{
    [Route("api/v1/department")]
    public class DepartmentController : DongNguyenController<DepartmentResource, CreateDepartmentResource, UpdateDepartmentResource, Department>
    {
        #region Constructor
        public DepartmentController(IDepartmentService departmentService,
            IMapper mapper,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(departmentService, mapper, responseMessage)
        {
        }
        #endregion

        #region Action
        [HttpGet]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<DepartmentResource>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<DepartmentResource>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<DepartmentResource>>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> GetAllAsync()
        {
            Log.Information($"{User.Identity?.Name}: get all department data.");

            return await base.GetAllAsync();
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}")]
        [ProducesResponseType(typeof(BaseResponse<DepartmentResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<DepartmentResource>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<DepartmentResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> GetByIdAsync(int id)
        {
            Log.Information($"{User.Identity?.Name}: get a department with Id is {id}.");

            return await base.GetByIdAsync(id);
        }

        [HttpPost]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}")]
        [ProducesResponseType(typeof(BaseResponse<DepartmentResource>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<DepartmentResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> CreateAsync([FromBody] CreateDepartmentResource resource)
        {
            Log.Information($"{User.Identity?.Name}: create a department.");

            return await base.CreateAsync(resource);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}")]
        [ProducesResponseType(typeof(BaseResponse<DepartmentResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<DepartmentResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateDepartmentResource resource)
        {
            Log.Information($"{User.Identity?.Name}: update a department with Id is {id}.");

            return await base.UpdateAsync(id, resource);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}")]
        [ProducesResponseType(typeof(BaseResponse<DepartmentResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<DepartmentResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            Log.Information($"{User.Identity?.Name}: delete a department with Id is {id}.");

            return await base.DeleteAsync(id);
        }
        #endregion
    }
}
