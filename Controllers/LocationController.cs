#nullable enable
using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Domain.Services;
using HR_Management.Resources;
using HR_Management.Resources.Location;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Controllers
{
    [Route("api/location")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly IMapper _mapper;

        public LocationController(ILocationService locationService, IMapper mapper)
        {
            _locationService = locationService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all record in table Location
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LocationResource>), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> GetAllWithPersonIdAsync()
        {
            var result = await _locationService.ListAsync();
            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }
            var resources = _mapper.Map<IEnumerable<Location>, IEnumerable<LocationResource>>(result.Object as IEnumerable<Location>);

            return Ok(resources);
        }

        /// <summary>
        /// Create a new record into table Location
        /// </summary>
        /// <param name="resource">Location data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(LocationResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> CreateLocationAsync([FromBody] CreateLocationResource resource)
        {
            var location = _mapper.Map<CreateLocationResource, Location>(resource);
            var result = await _locationService.CreateAsync(location);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var locationResource = _mapper.Map<Location, LocationResource>(result.Resource);
            return StatusCode(201, locationResource);
        }

        /// <summary>
        /// Update a record into Location by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(LocationResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> UpdateLocationAsync(int id, [FromBody] CreateLocationResource resource)
        {
            var location = _mapper.Map<CreateLocationResource, Location>(resource);
            var result = await _locationService.UpdateAsync(id, location);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var locationResource = _mapper.Map<Location, LocationResource>(result.Resource);
            return Ok(locationResource);
        }

        /// <summary>
        /// Delete a record in Location by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(LocationResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> DeleteLocationAsync(int id)
        {
            var result = await _locationService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var locationResource = _mapper.Map<Location, LocationResource>(result.Resource);
            return Ok(locationResource);
        }
    }
}
