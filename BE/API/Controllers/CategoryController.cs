﻿using AutoMapper;
using Business.Communication;
using Business.Data;
using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Extensions;
using Business.Resources;
using Business.Resources.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

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
            this._categoryService = categoryService;
            this._categoryRepository = categoryRepository;
        }
        #endregion

        #region Property
        private readonly ICategoryService _categoryService;
        private readonly ICategoryRepository _categoryRepository;
        #endregion

        #region Action
        [HttpGet]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<CategoryResource>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<CategoryResource>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<CategoryResource>>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> GetAllAsync()
        {
            Log.Information($"{User.Identity?.Name}: get all category data.");

            return await base.GetAllAsync();
        }

        [HttpPost("pagination")]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<CategoryResource>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<CategoryResource>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<CategoryResource>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPaginationWithFilterAsync([FromQuery] int page, [FromQuery] int pageSize, [FromBody] FilterCategoryResource filterResource)
        {
            Log.Information($"{User.Identity?.Name}: get pagination category.");

            QueryResource pagintation = new QueryResource(page, pageSize);

            var result = await _categoryService.GetPaginationAsync(pagintation, filterResource);

            if (!result.Success)
                return BadRequest(result);

            if (result.Resource is null)
                return NoContent();

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<CategoryResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CategoryResource>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<CategoryResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> GetByIdAsync(int id)
        {
            Log.Information($"{User.Identity?.Name}: get a category with Id is {id}.");

            return await base.GetByIdAsync(id);
        }

        [HttpGet("search")]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
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
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<CategoryResource>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<CategoryResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> CreateAsync([FromBody] CreateCategoryResource resource)
        {
            Log.Information($"{User.Identity?.Name}: create a {resource.Name} category.");

            return await base.CreateAsync(resource);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<CategoryResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CategoryResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateCategoryResource resource)
        {
            Log.Information($"{User.Identity?.Name}: update a category with Id is {id}.");

            return await base.UpdateAsync(id, resource);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
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
