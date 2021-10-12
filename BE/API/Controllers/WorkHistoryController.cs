﻿using Business.Communication;
using Business.Domain.Models;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.WorkHistory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/v1/work-history")]
    public class WorkHistoryController : DongNguyenController<WorkHistoryResource, CreateWorkHistoryResource, UpdateWorkHistoryResource, WorkHistory>
    {
        #region Constructor
        public WorkHistoryController(IWorkHistoryService workHistoryService,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(workHistoryService, responseMessage)
        {
        }
        #endregion

        #region Action
        [HttpPost]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<WorkHistoryResource>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<WorkHistoryResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> CreateAsync([FromBody] CreateWorkHistoryResource resource)
            => await base.CreateAsync(resource);

        [HttpPut("{id:int}")]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<WorkHistoryResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<WorkHistoryResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateWorkHistoryResource resource)
            => await base.UpdateAsync(id, resource);

        [HttpPut]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<WorkHistoryResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<WorkHistoryResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> SwapAsync([FromBody] SwapResource resource)
            => await base.SwapAsync(resource);

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BaseResponse<WorkHistoryResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<WorkHistoryResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> DeleteAsync(int id)
            => await base.DeleteAsync(id);
        #endregion
    }
}
