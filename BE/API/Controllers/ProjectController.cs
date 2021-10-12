using Business.Communication;
using Business.Domain.Models;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/v1/project")]
    public class ProjectController : DongNguyenController<ProjectResource, CreateProjectResource, UpdateProjectResource, Project>
    {
        #region Constructor
        public ProjectController(IProjectService projectService,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(projectService, responseMessage)
        {
        }
        #endregion

        #region Action
        [HttpPost]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<ProjectResource>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<ProjectResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> CreateAsync([FromBody] CreateProjectResource resource)
            => await base.CreateAsync(resource);

        [HttpPut("{id:int}")]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<ProjectResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ProjectResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateProjectResource resource)
            => await base.UpdateAsync(id, resource);

        [HttpPut]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<ProjectResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ProjectResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> SwapAsync([FromBody] SwapResource resource)
            => await base.SwapAsync(resource);

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BaseResponse<ProjectResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<ProjectResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> DeleteAsync(int id)
            => await base.DeleteAsync(id);
        #endregion
    }
}
