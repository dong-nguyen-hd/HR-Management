#nullable enable
using HR_Management.Domain.Services;
using HR_Management.Resources;
using HR_Management.Resources.Project;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Controllers
{
    [Route("api/project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// Get all record with personId in table Project
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet("{personId}")]
        [ProducesResponseType(typeof(IEnumerable<ProjectResource>), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> GetAllWithPersonIdAsync(int personId)
        {
            var result = await _projectService.ListAsync(personId);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return Ok(result.Resource);
        }

        /// <summary>
        /// Create a new record into table Project
        /// </summary>
        /// <param name="resource">Project data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProjectResource), 201)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> CreateProjectAsync([FromBody] CreateProjectResource resource)
        {
            var result = await _projectService.CreateAsync(resource);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return StatusCode(201, result.Resource);
        }

        /// <summary>
        /// Update a record into Project by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProjectResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> UpdateProjectAsync(int id, [FromBody] UpdateProjectResource resource)
        {
            var result = await _projectService.UpdateAsync(id, resource);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return Ok(result.Resource);
        }

        /// <summary>
        /// Swap OrderIndex value 
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ProjectResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> SwapProjectAsync([FromBody] SwapResource resource)
        {
            var result = await _projectService.SwapAsync(resource);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));
            
            return Ok(new ResultResource(result.Message, true));
        }

        /// <summary>
        /// Delete a record into Project by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ProjectResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> DeleteProjectAsync(int id)
        {
            var result = await _projectService.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return Ok(result.Resource);
        }
    }
}
