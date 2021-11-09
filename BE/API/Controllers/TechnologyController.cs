﻿using Business.Communication;
using Business.Domain.Models;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Technology;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/v1/technology")]
    public class TechnologyController : DongNguyenController<TechnologyResource, CreateTechnologyResource, UpdateTechnologyResource, Technology>
    {
        #region Constructor
        public TechnologyController(ITechnologyService technologyService,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(technologyService, responseMessage)
        {
            this._technologyService = technologyService;
        }
        #endregion

        #region Property
        private readonly ITechnologyService _technologyService;
        #endregion

        #region Action
        [HttpGet]
        [Authorize(Roles = "viewer, editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<TechnologyResource>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<TechnologyResource>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<TechnologyResource>>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> GetAllAsync()
            => await base.GetAllAsync();

        [HttpGet("{categoryId:int}")]
        [Authorize(Roles = "viewer, editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<TechnologyResource>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<TechnologyResource>>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<TechnologyResource>>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByCategoryAsync(int categoryId)
        {
            var result = await _technologyService.GetByCategoryAsync(categoryId);

            if (!result.Success)
                return BadRequest(result);

            if (result.Resource is null)
                return NoContent();

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<TechnologyResource>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<TechnologyResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> CreateAsync([FromBody] CreateTechnologyResource resource)
            => await base.CreateAsync(resource);

        [HttpPut("{id:int}")]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<TechnologyResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<TechnologyResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateTechnologyResource resource)
            => await base.UpdateAsync(id, resource);

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BaseResponse<TechnologyResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<TechnologyResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> DeleteAsync(int id)
            => await base.DeleteAsync(id);
        #endregion
    }
}