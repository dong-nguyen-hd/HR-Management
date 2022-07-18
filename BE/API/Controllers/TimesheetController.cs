using Business.Communication;
using Business.Domain.Services;
using Business.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/timesheet")]
    public class TimesheetController : ControllerBase
    {
        #region Property
        private readonly ITimesheetService _timesheetService;
        protected readonly ResponseMessage ResponseMessage;
        #endregion

        #region Constructor
        public TimesheetController(ITimesheetService timesheetService,
            IOptionsMonitor<ResponseMessage> responseMessage)
        {
            this._timesheetService = timesheetService;
            this.ResponseMessage = responseMessage.CurrentValue;
        }
        #endregion

        #region Action
        [HttpPost("import")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BaseResponse<>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ImportAsync([FromForm] IFormFile file)
        {
            // Validate file import
            var validateResult = ValidateTimesheet(file);
            if (!validateResult.isSuccess) return BadRequest(validateResult.result);

            var filePath = Path.GetTempFileName();

            var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            stream.Position = 0;

            var result = await _timesheetService.ImportAsync(stream);
            stream.Dispose();

            // Clean temp-file
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
        #endregion

        #region Private work
        private (bool isSuccess, BaseResponse<object> result) ValidateTimesheet(IFormFile file)
        {
            if (file == null || file.Length <= 0)
                return (false, new BaseResponse<object>(ResponseMessage.Values["File_Empty"]));

            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                return (false, new BaseResponse<object>(ResponseMessage.Values["Not_Support_File"]));

            return (true, new BaseResponse<object>(true));
        }
        #endregion
    }
}
