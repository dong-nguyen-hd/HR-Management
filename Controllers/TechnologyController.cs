#nullable enable
using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Domain.Services;
using HR_Management.Resources;
using HR_Management.Resources.Technology;
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
        private readonly IMapper _mapper;

        public TechnologyController(ITechnologyService technologyService, IMapper mapper)
        {
            _technologyService = technologyService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all record in table Technology
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TechnologyResource>), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> GetAllWithPersonIdAsync()
        {
            var result = await _technologyService.ListAsync();
            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }
            var resources = _mapper.Map<IEnumerable<Technology>, IEnumerable<TechnologyResource>>(result.Object as IEnumerable<Technology>);

            return Ok(resources);
        }

        /// <summary>
        /// Get all record with categoryId in table Technology
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet("{categoryId}")]
        [ProducesResponseType(typeof(IEnumerable<TechnologyResource>), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> GetAllWithPersonIdAsync(int categoryId)
        {
            var result = await _technologyService.ListAsync(categoryId);
            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }
            var resources = _mapper.Map<IEnumerable<Technology>, IEnumerable<TechnologyResource>>(result.Object as IEnumerable<Technology>);

            return Ok(resources);
        }

        /// <summary>
        /// Create a new record into table Technology
        /// </summary>
        /// <param name="resource">Technology data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TechnologyResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> CreateTechnologyAsync([FromBody] CreateTechnologyResource resource)
        {
            var technology = _mapper.Map<CreateTechnologyResource, Technology>(resource);
            var result = await _technologyService.CreateAsync(technology);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var technologyResource = _mapper.Map<Technology, TechnologyResource>(result.Resource);
            return StatusCode(201, technologyResource);
        }

        /// <summary>
        /// Update a record into Technology by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(TechnologyResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> UpdateTechnologyAsync(int id, [FromBody] UpdateTechnologyResource resource)
        {
            var technology = _mapper.Map<UpdateTechnologyResource, Technology>(resource);
            var result = await _technologyService.UpdateAsync(id, technology);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var technologyResource = _mapper.Map<Technology, TechnologyResource>(result.Resource);
            return Ok(technologyResource);
        }

        /// <summary>
        /// Delete a record into Technology by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(TechnologyResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> DeleteTechnologyAsync(int id)
        {
            var result = await _technologyService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var technologyResource = _mapper.Map<Technology, TechnologyResource>(result.Resource);
            return Ok(technologyResource);
        }
    }
}
