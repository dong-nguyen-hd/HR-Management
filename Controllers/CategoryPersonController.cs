#nullable enable
using HR_Management.Domain.Services;
using HR_Management.Resources;
using HR_Management.Resources.CategoryPerson;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Controllers
{
    [Route("api/categoryPerson")]
    [ApiController]
    public class CategoryPersonController : ControllerBase
    {
        private readonly ICategoryPersonService _categoryPersonService;

        public CategoryPersonController(ICategoryPersonService categoryPersonService)
        {
            _categoryPersonService = categoryPersonService;
        }

        /// <summary>
        /// Get all record with personId in table CategoryPerson
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet("{personId}")]
        [ProducesResponseType(typeof(IEnumerable<CategoryPersonResource>), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> GetAllWithPersonIdAsync(int personId)
        {
            var result = await _categoryPersonService.ListAsync(personId);
            if (!result.Success)
            {
                return BadRequest(new ResultResource(result.Message));
            }

            return Ok(result.Resource);
        }

        /// <summary>
        /// Create a new record into table CategoryPerson
        /// </summary>
        /// <param name="resource">CategoryPerson data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CategoryPersonResource), 201)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> CreateCategoryPersonAsync([FromBody] CreateCategoryPersonResource resource)
        {
            var result = await _categoryPersonService.CreateAsync(resource);

            if (!result.Success)
            {
                return BadRequest(new ResultResource(result.Message));
            }

            return StatusCode(201, result.Resource);
        }

        /// <summary>
        /// Update a record into CategoryPerson by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CategoryPersonResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> UpdateCategoryPersonAsync(int id, [FromBody] UpdateCategoryPersonResource resource)
        {
            var result = await _categoryPersonService.UpdateAsync(id, resource);

            if (!result.Success)
            {
                return BadRequest(new ResultResource(result.Message));
            }

            return Ok(result.Resource);
        }

        /// <summary>
        /// Swap OrderIndex value 
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(CategoryPersonResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> SwapCategoryPersonAsync([FromBody] SwapResource resource)
        {
            var result = await _categoryPersonService.SwapAsync(resource);

            if (!result.Success)
            {
                return BadRequest(new ResultResource(result.Message));
            }

            return Ok(new ResultResource(result.Message, true));
        }

        /// <summary>
        /// Delete a record into CategoryPerson by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(CategoryPersonResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> DeleteCategoryPersonAsync(int id)
        {
            var result = await _categoryPersonService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ResultResource(result.Message));
            }

            return Ok(result.Resource);
        }
    }
}
