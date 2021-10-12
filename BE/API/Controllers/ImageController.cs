using Business.Communication;
using Business.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/v1/image")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        #region Property
        private readonly IImageService _imageService;
        #endregion

        #region Constructor
        public ImageController(IImageService imageService)
        {
            this._imageService = imageService;
        }
        #endregion

        #region Action
        [HttpPut("{personId:int}")]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(BaseResponse<>), 200)]
        [ProducesResponseType(typeof(BaseResponse<>), 400)]
        public async Task<IActionResult> SaveImageAsync(int personId, [FromForm] IFormFile image)
        {
            var filePath = Path.GetTempFileName();

            var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);
            var result = await _imageService.SaveImageAsync(personId, stream);
            stream.Dispose();

            // Clean temp-file
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
        #endregion
    }
}
