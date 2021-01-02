#nullable enable
using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Domain.Services;
using HR_Management.Resources;
using HR_Management.Resources.WorkHistory;
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
        private readonly IMapper _mapper;

        public WorkHistoryController(IWorkHistoryService workHistoryService, IMapper mapper)
        {
            _workHistoryService = workHistoryService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all record with personId in table WorkHistory
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet("{personId}")]
        [ProducesResponseType(typeof(IEnumerable<WorkHistoryResource>), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> GetAllWithPersonIdAsync(int personId)
        {
            var result = await _workHistoryService.ListAsync(personId);
            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }
            var resources = _mapper.Map<IEnumerable<WorkHistory>, IEnumerable<WorkHistoryResource>>(result.Object as IEnumerable<WorkHistory>);

            return Ok(resources);
        }

        /// <summary>
        /// Create a new record into table WorkHistory
        /// </summary>
        /// <param name="resource">WorkHistory data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(WorkHistoryResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> CreateWorkHistoryAsync([FromBody] CreateWorkHistoryResource resource)
        {
            var workHistory = _mapper.Map<CreateWorkHistoryResource, WorkHistory>(resource);
            var result = await _workHistoryService.CreateAsync(workHistory);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var workHistoryResource = _mapper.Map<WorkHistory, WorkHistoryResource>(result.Resource);
            return StatusCode(201, workHistoryResource);
        }

        /// <summary>
        /// Update a record into WorkHistory by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(WorkHistoryResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> UpdateWorkHistoryAsync(int id, [FromBody] UpdateWorkHistoryResource resource)
        {
            var workHistory = _mapper.Map<UpdateWorkHistoryResource, WorkHistory>(resource);
            var result = await _workHistoryService.UpdateAsync(id, workHistory);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var workHistoryResource = _mapper.Map<WorkHistory, WorkHistoryResource>(result.Resource);
            return Ok(workHistoryResource);
        }

        /// <summary>
        /// Swap OrderIndex value 
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(WorkHistoryResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> SwapWorkHistoryAsync([FromBody] SwapResource resource)
        {
            var result = await _workHistoryService.SwapAsync(resource);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }
            var resources = _mapper.Map<IEnumerable<WorkHistory>, IEnumerable<WorkHistoryResource>>(result.Object as IEnumerable<WorkHistory>);

            return Ok(resources);
        }

        /// <summary>
        /// Delete a record into WorkHistory by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(WorkHistoryResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> DeleteWorkHistoryAsync(int id)
        {
            var result = await _workHistoryService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var workHistoryResource = _mapper.Map<WorkHistory, WorkHistoryResource>(result.Resource);
            return Ok(workHistoryResource);
        }
    }
}
