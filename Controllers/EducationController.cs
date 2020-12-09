using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Domain.Services;
using HR_Management.Resources;
using Microsoft.AspNetCore.Mvc;

namespace HR_Management.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet]
        public async Task<IEnumerable<EducationResource>> GetAllAsync()
        {
            var listEducation = await _educationService.ListAsync();
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
