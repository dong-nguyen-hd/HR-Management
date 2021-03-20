#nullable enable
using HR_Management.Domain.Services;
using HR_Management.Extensions;
using HR_Management.Resources;
using HR_Management.Resources.Person;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HR_Management.Controllers
{
    [Route("api/person")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        /// <summary>
        /// Get a record with Id in table Person
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet("{personId:int}")]
        [ProducesResponseType(typeof(PersonResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> GetWithIdAsync(int personId)
        {
            bool isMobile = HttpContext.Request.Headers["User-Agent"].ToString().IsMobile();
            var result = await _personService.FindByIdAsync(personId, isMobile);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return Ok(result.Resource);
        }

        /// <summary>
        /// Create a new record into table Person
        /// </summary>
        /// <param name="resource">Person data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(PersonResource), 201)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> CreatePersonAsync([FromBody] CreatePersonResource resource)
        {
            bool isMobile = HttpContext.Request.Headers["User-Agent"].ToString().IsMobile();
            var result = await _personService.CreateAsync(resource, isMobile);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return StatusCode(201, result.Resource);
        }

        /// <summary>
        /// Update a record into Person by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(PersonResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> UpdatePersonAsync([FromBody] UpdatePersonResource resource, int id)
        {
            bool isMobile = HttpContext.Request.Headers["User-Agent"].ToString().IsMobile();
            var result = await _personService.UpdateAsync(id, resource, isMobile);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return Ok(result.Resource);
        }

        /// <summary>
        /// Assign component of OrderIndex
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("swap/{personId:int}")]
        [ProducesResponseType(typeof(PersonResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> AssignComponentAsync(int personId, [FromBody] ComponentResource resource)
        {
            var result = await _personService.AssignComponentAsync(personId, resource);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return Ok(result.Resource);
        }

        /// <summary>
        /// Delete a record into Person by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(PersonResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> DeletePersonAsync(int id)
        {
            bool isMobile = HttpContext.Request.Headers["User-Agent"].ToString().IsMobile();
            var result = await _personService.DeleteAsync(id, isMobile);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return Ok(result.Resource);
        }
    }
}
