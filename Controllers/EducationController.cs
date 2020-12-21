#nullable enable
using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Domain.Services;
using HR_Management.Resources;
using HR_Management.Resources.Education;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Controllers
{
    [Route("api/education")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly IEducationService _educationService;
        private readonly IMapper _mapper;

        public EducationController(IEducationService educationService, IMapper mapper)
        {
            _educationService = educationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all record with personId in table Education
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet("{personId}")]
        [ProducesResponseType(typeof(IEnumerable<EducationResource>), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> GetAllWithPersonIdAsync(int personId)
        {
            var result = await _educationService.ListAsync(personId);
            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }
            var resources = _mapper.Map<IEnumerable<Education>, IEnumerable<EducationResource>>(result.Object as IEnumerable<Education>);

            return Ok(resources);
        }

        /// <summary>
        /// Create a new record into table Education
        /// </summary>
        /// <param name="resource">Education data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(EducationResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> CreateEducationAsync([FromBody] CreateEducationResource resource)
        {
            var education = _mapper.Map<CreateEducationResource, Education>(resource);
            var result = await _educationService.CreateAsync(education);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var educationResource = _mapper.Map<Education, EducationResource>(result.Resource);
            return StatusCode(201, educationResource);
        }

        /// <summary>
        /// Update a record into Education by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(EducationResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> UpdateEducationAsync(int id, [FromBody] UpdateEducationResource resource)
        {
            var education = _mapper.Map<UpdateEducationResource, Education>(resource);
            var result = await _educationService.UpdateAsync(id, education);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var educationResource = _mapper.Map<Education, EducationResource>(result.Resource);
            return Ok(educationResource);
        }

        /// <summary>
        /// Delete a record into Education by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(EducationResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> DeleteEducationAsync(int id)
        {
            var result = await _educationService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var educationResource = _mapper.Map<Education, EducationResource>(result.Resource);
            return Ok(educationResource);
        }
    }
}
