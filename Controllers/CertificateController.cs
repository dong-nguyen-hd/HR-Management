#nullable enable
using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Domain.Services;
using HR_Management.Resources;
using HR_Management.Resources.Certificate;
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
        private readonly IMapper _mapper;

        public CertificateController(ICertificateService certificateService, IMapper mapper)
        {
            _certificateService = certificateService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all record with personId in table Certificate
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet("{personId}")]
        [ProducesResponseType(typeof(IEnumerable<CertificateResource>), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> GetAllWithPersonIdAsync(int personId)
        {
            var result = await _certificateService.ListAsync(personId);
            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }
            var resources = _mapper.Map<IEnumerable<Certificate>, IEnumerable<CertificateResource>>(result.Object as IEnumerable<Certificate>);

            return Ok(resources);
        }

        /// <summary>
        /// Create a new record into table Certificate
        /// </summary>
        /// <param name="resource">Certificate data.</param>
        /// <returns>Response for the request.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CertificateResource), 201)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> CreateCertificateAsync([FromBody] CreateCertificateResource resource)
        {
            var certificate = _mapper.Map<CreateCertificateResource, Certificate>(resource);
            var result = await _certificateService.CreateAsync(certificate);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var certificateResource = _mapper.Map<Certificate, CertificateResource>(result.Resource);
            return StatusCode(201, certificateResource);
        }

        /// <summary>
        /// Update a record into Certificate by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CertificateResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> UpdateCertificateAsync(int id, [FromBody] UpdateCertificateResource resource)
        {
            var certificate = _mapper.Map<UpdateCertificateResource, Certificate>(resource);
            var result = await _certificateService.UpdateAsync(id, certificate);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var certificateResource = _mapper.Map<Certificate, CertificateResource>(result.Resource);
            return Ok(certificateResource);
        }

        /// <summary>
        /// Swap OrderIndex value 
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(CertificateResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> SwapCertificateAsync([FromBody] SwapResource resource)
        {
            var result = await _certificateService.SwapAsync(resource);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }
            var resources = _mapper.Map<IEnumerable<Certificate>, IEnumerable<CertificateResource>>(result.Object as IEnumerable<Certificate>);

            return Ok(resources);
        }

        /// <summary>
        /// Delete a record into Certificate by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(CertificateResource), 200)]
        [ProducesResponseType(typeof(ErrorResource), 400)]
        public async Task<IActionResult> DeleteCertificateAsync(int id)
        {
            var result = await _certificateService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(new ErrorResource(result.Message));
            }

            var certificateResource = _mapper.Map<Certificate, CertificateResource>(result.Resource);
            return Ok(certificateResource);
        }
    }
}
