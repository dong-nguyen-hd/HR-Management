﻿using AutoMapper;
using Business.Communication;
using Business.Domain.Models;
using Business.Domain.Services;
using Business.Resources;
using Business.Data;
using Business.Resources.Person;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

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
            IMapper mapper,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(personService, mapper, responseMessage)
        {
            this._personService = personService;
            this._imageService = imageService;
        }
        #endregion

        #region Action
        [HttpPut("image/{id:int}")]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<>), 200)]
        [ProducesResponseType(typeof(BaseResponse<>), 400)]
        public async Task<IActionResult> SaveImageAsync(int id, [FromForm] IFormFile image)
        {
            Log.Information($"{User.Identity?.Name}: save image a person with Id is {id}.");

            var filePath = Path.GetTempFileName();

            var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);
            stream.Position = 0;

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
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PersonResource>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PersonResource>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PersonResource>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPaginationAsync([FromQuery] int page, [FromQuery] int pageSize)
        {
            Log.Information($"{User.Identity?.Name}: get pagination person.");

            QueryResource pagintation = new QueryResource(page, pageSize);

            var result = await _personService.GetPaginationAsync(pagintation, null);

            if (!result.Success)
                return BadRequest(result);

            if (result.Resource is null)
                return NoContent();

            return Ok(result);
        }

        [HttpPost("pagination")]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PersonResource>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PersonResource>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<PersonResource>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPaginationWithFilterAsync([FromQuery] int page, [FromQuery] int pageSize, [FromBody] FilterPersonResource filterResource)
        {
            Log.Information($"{User.Identity?.Name}: get pagination person.");

            QueryResource pagintation = new QueryResource(page, pageSize);

            var result = await _personService.GetPaginationAsync(pagintation, filterResource);

            if (!result.Success)
                return BadRequest(result);

            if (result.Resource is null)
                return NoContent();

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> GetByIdAsync(int id)
        {
            Log.Information($"{User.Identity?.Name}: get a person with Id is {id}.");

            return await base.GetByIdAsync(id);
        }

        [HttpPost]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}")]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> CreateAsync([FromBody] CreatePersonResource resource)
        {
            Log.Information($"{User.Identity?.Name}: create a person.");

            resource.CreatedBy = User.Identity?.Name;

            var insertResult = await _personService.InsertAsync(resource);

            if(!insertResult.Success)
                return BadRequest(insertResult);

            return StatusCode(201, insertResult);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdatePersonResource resource)
        {
            Log.Information($"{User.Identity?.Name}: update a person with Id is {id}.");

            return await base.UpdateAsync(id, resource);
        }

        [HttpPut("reset-avatar/{id:int}")]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetAvatarAsync(int id)
        {
            Log.Information($"{User.Identity?.Name}: reset avatar of the person with Id is {id}.");

            var result = await _imageService.ResetPersonAvatarAsync(id);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorQTNS}, {Role.EditorQTDA}, {Role.Viewer}")]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<PersonResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            Log.Information($"{User.Identity?.Name}: delete a person with Id is {id}.");

            return await base.DeleteAsync(id);
        }
        #endregion
    }
}
