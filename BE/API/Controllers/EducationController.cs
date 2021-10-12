using Business.Communication;
using Business.Domain.Models;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Education;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/v1/education")]
    public class EducationController : DongNguyenController<EducationResource, CreateEducationResource, UpdateEducationResource, Education>
    {
        #region Constructor
        public EducationController(IEducationService educationService,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(educationService, responseMessage)
        {
        }
        #endregion

        #region Action
        [HttpPost]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<EducationResource>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<EducationResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> CreateAsync([FromBody] CreateEducationResource resource)
            => await base.CreateAsync(resource);

        [HttpPut("{id:int}")]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<EducationResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<EducationResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateEducationResource resource)
            => await base.UpdateAsync(id, resource);

        [HttpPut]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<EducationResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<EducationResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> SwapAsync([FromBody] SwapResource resource)
            => await base.SwapAsync(resource);

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BaseResponse<EducationResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<EducationResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> DeleteAsync(int id)
            => await base.DeleteAsync(id);
        #endregion
    }
}
