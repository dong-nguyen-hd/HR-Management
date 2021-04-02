#nullable enable
using HR_Management.Domain.Services;
using HR_Management.Extensions;
using HR_Management.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace HR_Management.Controllers
{
    [Route("api/image")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        /// <summary>
        /// Upload image to server
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPut("{personId:int}")]
        [Authorize(Roles = "editor, admin")]
        [ProducesResponseType(typeof(ResultResource), 200)]
        [ProducesResponseType(typeof(ResultResource), 400)]
        public async Task<IActionResult> SaveImage(int personId, [FromForm] IFormFile image)
        {
            var filePath = Path.GetTempFileName();

            var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream);
            bool isMobile = HttpContext.Request.Headers["User-Agent"].ToString().IsMobile();
            var response = await _imageService.SaveImage(personId, stream, isMobile);
            stream.Dispose();

            // Clean temp-file
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            if (!response.Success)
                return BadRequest(new ResultResource(response.Message));

            return Ok(response.Resource);
        }
    }
}
