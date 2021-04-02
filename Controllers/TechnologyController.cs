#nullable enable
using HR_Management.Domain.Services;
using HR_Management.Resources;
using HR_Management.Resources.Technology;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Controllers
{
    [Route("api/technology")]
    [ApiController]
    public class TechnologyController : ControllerBase
    {
        private readonly ITechnologyService _technologyService;

        public TechnologyController(ITechnologyService technologyService)
        {
            _technologyService = technologyService;
        }

        /// <summary>
        /// Get all record in table Technology
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "viewer, editor, admin")]
        [ProducesResponseType(typeof(IEnumerable<TechnologyResource>), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _technologyService.ListAsync();

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return Ok(result.Resource);
        }

        /// <summary>
        /// Get all record with categoryId in table Technology
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("{categoryId}")]
        [Authorize(Roles = "viewer, editor, admin")]
        [ProducesResponseType(typeof(IEnumerable<TechnologyResource>), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> GetAllWithCategoryIdAsync(int categoryId)
        {
            var result = await _technologyService.ListAsync(categoryId);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return Ok(result.Resource);
        }

        /// <summary>
        /// Create a new record into table Technology
        /// </summary>
        /// <param name="resource">Technology data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPost]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(TechnologyResource), 201)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> CreateTechnologyAsync([FromBody] CreateTechnologyResource resource)
        {
            var result = await _technologyService.CreateAsync(resource);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return StatusCode(201, result.Resource);
        }

        /// <summary>
        /// Update a record into Technology by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(TechnologyResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> UpdateTechnologyAsync(int id, [FromBody] UpdateTechnologyResource resource)
        {
            var result = await _technologyService.UpdateAsync(id, resource);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return Ok(result.Resource);
        }

        /// <summary>
        /// Delete a record into Technology by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(TechnologyResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> DeleteTechnologyAsync(int id)
        {
            var result = await _technologyService.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return Ok(result.Resource);
        }
    }
}
