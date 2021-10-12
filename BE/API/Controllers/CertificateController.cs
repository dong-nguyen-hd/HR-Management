using Business.Communication;
using Business.Domain.Models;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Certificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/v1/certificate")]
    public class CertificateController : DongNguyenController<CertificateResource, CreateCertificateResource, UpdateCertificateResource, Certificate>
    {
        #region Constructor
        public CertificateController(ICertificateService certificateService,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(certificateService, responseMessage)
        {
        }
        #endregion

        #region Action
        [HttpPost]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<CertificateResource>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<CertificateResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> CreateAsync([FromBody] CreateCertificateResource resource)
            => await base.CreateAsync(resource);

        [HttpPut("{id:int}")]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<CertificateResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CertificateResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> UpdateAsync(int id, [FromBody] UpdateCertificateResource resource)
            => await base.UpdateAsync(id, resource);

        [HttpPut]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<CertificateResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CertificateResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> SwapAsync([FromBody] SwapResource resource)
            => await base.SwapAsync(resource);

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(BaseResponse<CertificateResource>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<CertificateResource>), StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> DeleteAsync(int id)
            => await base.DeleteAsync(id);
        #endregion
    }
}
