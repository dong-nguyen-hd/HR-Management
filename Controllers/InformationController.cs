#nullable enable
using HR_Management.Domain.Services;
using HR_Management.Resources;
using HR_Management.Resources.Information;
using HR_Management.Resources.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Controllers
{
    [Route("api/information")]
    [ApiController]
    public class InformationController : ControllerBase
    {
        private readonly IInformationService _informationService;

        public InformationController(IInformationService informationService)
        {
            _informationService = informationService;
        }

        /// <summary>
        /// Get all record with pagination
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InformationResource>), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> GetAllAsync([FromQuery] int page, [FromQuery] int pageSize)
        {
            QueryResource pagintation = new QueryResource(page, pageSize);

            string route = Request.Path.Value;

            var result = await _informationService.ListWithPaginationAsync(pagintation, route);

            if (!result.Success)
                return BadRequest(new ResultResource(result.Message));

            return Ok(result);
        }
    }
}
