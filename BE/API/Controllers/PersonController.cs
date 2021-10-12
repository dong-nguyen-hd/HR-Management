using Business.Communication;
using Business.Domain.Models;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Person;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/v1/person")]
    public class PersonController : DongNguyenController<PersonResource, CreatePersonResource, UpdatePersonResource, Person>
    {
        #region Property
        private readonly IPersonService _personService;
        #endregion

        #region Constructor
        public PersonController(IPersonService personService,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(personService, responseMessage)
        {
            this._personService = personService;
        }
        #endregion

        #region Action
        [HttpGet]
        [Authorize(Roles = "viewer, editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PersonResource>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PersonResource>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PersonResource>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPaginationAsync([FromQuery] int page, [FromQuery] int pageSize)
        {
            QueryResource pagintation = new QueryResource(page, pageSize);

            var result = await _personService.GetPaginationAsync(pagintation);

            if (!result.Success)
                return BadRequest(result);

            if (result.Resource is null)
                return NoContent();

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "viewer, editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> GetByIdAsync(int id)
            => await base.GetByIdAsync(id);

        [HttpPost]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> CreateAsync([FromBody] CreatePersonResource resource)
        {
            resource.CreatedBy = User.Identity?.Name;

            return await base.CreateAsync(resource);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePersonAsync(int id, [FromBody] UpdatePersonResource resource)
            => await base.UpdateAsync(id, resource);

        [HttpPut("swap/{id:int}")]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignComponentAsync(int id, [FromBody] ComponentResource resource)
        {
            var result = await _personService.AssignComponentAsync(id, resource);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> DeleteAsync(int id)
            => await base.DeleteAsync(id);
        #endregion
    }
}
