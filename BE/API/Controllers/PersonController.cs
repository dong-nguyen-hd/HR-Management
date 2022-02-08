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
using System.IO;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/v1/person")]
    public class PersonController : DongNguyenController<PersonResource, CreatePersonResource, UpdatePersonResource, Person>
    {
        #region Property
        private readonly IPersonService _personService;
        private readonly IImageService _imageService;
        #endregion

        #region Constructor
        public PersonController(IPersonService personService,
            IImageService imageService,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(personService, responseMessage)
        {
            this._personService = personService;
            this._imageService = imageService;
        }
        #endregion

        #region Action
        [HttpPut("image/{id:int}")]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<>), 200)]
        [ProducesResponseType(typeof(BaseResponse<>), 400)]
        public async Task<IActionResult> SaveImageAsync(int id, [FromForm] IFormFile image)
        {
            var filePath = Path.GetTempFileName();

            var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);
            var result = await _imageService.SaveImagePersonAsync(id, stream);
            stream.Dispose();

            // Clean temp-file
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

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

            var result = await _personService.InsertAsync(resource);

            if (result.Success)
                return StatusCode(201, result);

            return BadRequest(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePersonAsync(int id, [FromBody] UpdatePersonResource resource)
        {
            var result = await _personService.UpdateAsync(id, resource);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

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
