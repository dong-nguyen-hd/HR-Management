using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Domain.Services;
using HR_Management.Resources.Education;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Controllers
{
    [Route("api/education")]
    [Produces("application/json")]
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
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EducationResource>), 200)]
        public async Task<IEnumerable<EducationResource>> GetAllWithPersonIdAsync(int personId)
        {
            var listEducation = await _educationService.ListAsync(personId);
            var resources = _mapper.Map<IEnumerable<Education>, IEnumerable<EducationResource>>(listEducation);

            return resources;
        }

        //[HttpPost]
        //public async Task<IActionResult> PostAsync([FromBody] CreateEducationResource resource)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState.GetErrorMessages());
        //}
    }
}
