#nullable enable
using HR_Management.Domain.Services;
using HR_Management.Resources;
using HR_Management.Resources.WorkHistory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Controllers
{
    [Route("api/workHistory")]
    [ApiController]
    public class WorkHistoryController : ControllerBase
    {
        private readonly IWorkHistoryService _workHistoryService;

        public WorkHistoryController(IWorkHistoryService workHistoryService)
        {
            _workHistoryService = workHistoryService;
        }

        /// <summary>
        /// Get all record with personId in table WorkHistory
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet("{personId}")]
        [Authorize(Roles = "viewer, editor, admin")]
        [ProducesResponseType(typeof(IEnumerable<WorkHistoryResource>), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> GetAllWithPersonIdAsync(int personId)
        {
            var result = await _workHistoryService.ListAsync(personId);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return Ok(result.Resource);
        }

        /// <summary>
        /// Create a new record into table WorkHistory
        /// </summary>
        /// <param name="resource">WorkHistory data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPost]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(WorkHistoryResource), 201)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> CreateWorkHistoryAsync([FromBody] CreateWorkHistoryResource resource)
        {
            var result = await _workHistoryService.CreateAsync(resource);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return StatusCode(201, result.Resource);
        }

        /// <summary>
        /// Update a record into WorkHistory by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(WorkHistoryResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> UpdateWorkHistoryAsync(int id, [FromBody] UpdateWorkHistoryResource resource)
        {
            var result = await _workHistoryService.UpdateAsync(id, resource);

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
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(WorkHistoryResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> SwapWorkHistoryAsync([FromBody] SwapResource resource)
        {
            var result = await _workHistoryService.SwapAsync(resource);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return Ok(new ResultResource(result.Message, true));
        }

        /// <summary>
        /// Delete a record into WorkHistory by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(WorkHistoryResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> DeleteWorkHistoryAsync(int id)
        {
            var result = await _workHistoryService.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return Ok(result.Resource);
        }
    }
}
