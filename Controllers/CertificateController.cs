#nullable enable
using HR_Management.Domain.Services;
using HR_Management.Infrastructure;
using HR_Management.Resources;
using HR_Management.Resources.Certificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Controllers
{
    [Route("api/certificate")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificateService _certificateService;

        public CertificateController(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        /// <summary>
        /// Get all record with personId in table Certificate
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet("{personId}")]
        [Authorize(Roles = "viewer, editor, admin")]
        [ProducesResponseType(typeof(IEnumerable<CertificateResource>), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> GetAllWithPersonIdAsync(int personId)
        {
            var result = await _certificateService.ListAsync(personId);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));
            
            return Ok(result.Resource);
        }

        /// <summary>
        /// Create a new record into table Certificate
        /// </summary>
        /// <param name="resource">Certificate data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPost]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(CertificateResource), 201)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> CreateCertificateAsync([FromBody] CreateCertificateResource resource)
        {
            var result = await _certificateService.CreateAsync(resource);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return StatusCode(201, result.Resource);
        }

        /// <summary>
        /// Update a record into Certificate by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(CertificateResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> UpdateCertificateAsync([FromBody] UpdateCertificateResource resource, int id)
        {
            var result = await _certificateService.UpdateAsync(id, resource);

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
        [ProducesResponseType(typeof(CertificateResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> SwapCertificateAsync([FromBody] SwapResource resource)
        {
            var result = await _certificateService.SwapAsync(resource);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));
            
            return Ok(new ResultResource(result.Message, true));
        }

        /// <summary>
        /// Delete a record into Certificate by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(CertificateResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> DeleteCertificateAsync(int id)
        {
            var result = await _certificateService.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return Ok(result.Resource);
        }
    }
}
