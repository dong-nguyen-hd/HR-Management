#nullable enable
using HR_Management.Domain.Services;
using HR_Management.Resources;
using HR_Management.Resources.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get all record in table Category
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "viewer, editor, admin")]
        [ProducesResponseType(typeof(IEnumerable<CategoryResource>), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> GetAllWithPersonIdAsync()
        {
            var result = await _categoryService.ListAsync();

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return Ok(result.Resource);
        }

        /// <summary>
        /// Create a new record into table Category
        /// </summary>
        /// <param name="resource">Category data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPost]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(CategoryResource), 201)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CreateCategoryResource resource)
        {
            var result = await _categoryService.CreateAsync(resource);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return StatusCode(201, result.Resource);
        }

        /// <summary>
        /// Update a record into Category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(CategoryResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> UpdateCategoryAsync(int id, [FromBody] UpdateCategoryResource resource)
        {
            var result = await _categoryService.UpdateAsync(id, resource);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));
            
            return Ok(result.Resource);
        }
        
        /// <summary>
        /// Delete a record into Category by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(CategoryResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return Ok(result.Resource);
        }
    }
}
