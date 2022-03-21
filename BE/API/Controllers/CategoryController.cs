﻿using AutoMapper;
using Business.Communication;
using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Extensions;
using Business.Resources;
using Business.Resources.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/v1/category")]
    public class CategoryController : DongNguyenController<CategoryResource, CreateCategoryResource, UpdateCategoryResource, Category>
    {
        #region Constructor
        public CategoryController(ICategoryService categoryService,
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(categoryService, mapper, responseMessage)
        {
            this._categoryRepository = categoryRepository;
        }
        #endregion

        #region Property
        private readonly ICategoryRepository _categoryRepository;
        #endregion

        #region Action
        [HttpGet]
        [Authorize(Roles = "viewer, editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<CategoryResource>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<CategoryResource>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<CategoryResource>>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> GetAllAsync()
        {
            Log.Information($"{User.Identity?.Name}: get all category data.");

            return await base.GetAllAsync();
        }

        [HttpGet("search")]
        [Authorize(Roles = "viewer, editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<CategoryResource>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<CategoryResource>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<CategoryResource>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindAsync([FromQuery] string filterName)
        {
            Log.Information($"{User.Identity?.Name}: find category data with {filterName}-keyword.");

            var result = await _categoryRepository.FindByNameAsync(filterName.RemoveSpaceCharacter());

            if (result is null)
                return NoContent();

            return Ok(new BaseResponse<IEnumerable<CategoryResource>>(Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResource>>(result)));
        }

        [HttpPost]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<CategoryResource>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<CategoryResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> CreateAsync([FromBody] CreateCategoryResource resource)
        {
            Log.Information($"{User.Identity?.Name}: create a {resource.Name} category.");

            return await base.CreateAsync(resource);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<CategoryResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CategoryResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateCategoryResource resource)
        {
            Log.Information($"{User.Identity?.Name}: update a category with Id is {id}.");

            return await base.UpdateAsync(id, resource);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BaseResponse<CategoryResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CategoryResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            Log.Information($"{User.Identity?.Name}: delete a category with Id is {id}.");

            return await base.DeleteAsync(id);
        }
        #endregion
    }
}
