using Business.Communication;
using Business.Domain.Models;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.CategoryPerson;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/v1/category-person")]
    public class CategoryPersonController : DongNguyenController<CategoryPersonResource, CreateCategoryPersonResource, UpdateCategoryPersonResource, CategoryPerson>
    {
        #region Constructor
        public CategoryPersonController(ICategoryPersonService categoryPersonService,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(categoryPersonService, responseMessage)
        {
        }
        #endregion

        #region Action
        [HttpPost]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<CategoryPersonResource>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<CategoryPersonResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> CreateAsync([FromBody] CreateCategoryPersonResource resource)
            => await base.CreateAsync(resource);

        [HttpPut("{id:int}")]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<CategoryPersonResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CategoryPersonResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateCategoryPersonResource resource)
            => await base.UpdateAsync(id, resource);

        [HttpPut]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<CategoryPersonResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CategoryPersonResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> SwapAsync([FromBody] SwapResource resource)
            => await base.SwapAsync(resource);

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BaseResponse<CategoryPersonResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CategoryPersonResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> DeleteAsync(int id)
            => await base.DeleteAsync(id);
        #endregion
    }
}
