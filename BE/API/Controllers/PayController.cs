﻿using AutoMapper;
using Business.Communication;
using Business.Domain.Models;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Pay;
using Business.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using Business.Domain.Repositories;

namespace API.Controllers
{
    [Route("api/v1/pay")]
    public class PayController : DongNguyenController<PayResource, CreatePayResource, UpdatePayResource, Pay>
    {
        #region Property
        private readonly IPayRepository _payRepository;
        #endregion

        #region Constructor
        public PayController(IPayService payService,
            IPayRepository payRepository,
            IMapper mapper,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(payService, mapper, responseMessage)
        {
            this._payRepository = payRepository;
        }
        #endregion

        #region Action
        [HttpGet]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorKT}")]
        [ProducesResponseType(typeof(BaseResponse<PayResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<PayResource>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BaseResponse<PayResource>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByPersonIdAsync(int personId, DateTime date)
        {
            Log.Information($"{User.Identity?.Name}: get a pay with person-id:{personId} and date:{date}.");

            var result = await _payRepository.GetByPersonIdAsync(personId, date);

            if (result is null)
                return NoContent();

            return Ok(new BaseResponse<PayResource>(Mapper.Map<Pay, PayResource>(result)));
        }

        [HttpPost]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorKT}")]
        [ProducesResponseType(typeof(BaseResponse<PayResource>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<PayResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> CreateAsync([FromBody] CreatePayResource resource)
        {
            Log.Information($"{User.Identity?.Name}: create a pay.");

            return await base.CreateAsync(resource);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = $"{Role.Admin}, {Role.EditorKT}")]
        [ProducesResponseType(typeof(BaseResponse<PayResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<PayResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> DeleteAsync(int id)
        {
            Log.Information($"{User.Identity?.Name}: delete a pay with Id is {id}.");

            return await base.DeleteAsync(id);
        }
        #endregion
    }
}
