﻿using AutoMapper;
using Business.Communication;
using Business.Domain.Models;
using Business.Domain.Services;
using Business.Data;
using Business.Resources;
using Business.Resources.CategoryPerson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

namespace API.Controllers
{
    [Route("api/v1/category-person")]
    public class CategoryPersonController : DongNguyenController<CategoryPersonResource, CreateCategoryPersonResource, UpdateCategoryPersonResource, CategoryPerson>
    {
        #region Constructor
        public CategoryPersonController(ICategoryPersonService categoryPersonService,
            IMapper mapper,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(categoryPersonService, mapper, responseMessage)
        {
        }
        #endregion

        #region Action
        [HttpPost]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<CategoryPersonResource>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<CategoryPersonResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> CreateAsync([FromBody] CreateCategoryPersonResource resource)
        {
            Log.Information($"{User.Identity?.Name}: create a category-person.");

            return await base.CreateAsync(resource);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<CategoryPersonResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CategoryPersonResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateCategoryPersonResource resource)
        {
            Log.Information($"{User.Identity?.Name}: update a category-person.");

            return await base.UpdateAsync(id, resource);
        }

        [HttpPut]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<CategoryPersonResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CategoryPersonResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> ChangeOrderIndexAsync([FromBody] List<int> ids)
        {
            Log.Information($"{User.Identity?.Name}: change order-index a category-person.");

            return await base.ChangeOrderIndexAsync(ids);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<CategoryPersonResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CategoryPersonResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            Log.Information($"{User.Identity?.Name}: delete a category-person with Id is {id}.");

            return await base.DeleteAsync(id);
        }
        #endregion
    }
}
