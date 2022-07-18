using AutoMapper;
using Business.Communication;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Information;
using Business.Resources.Timesheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using OfficeOpenXml;

namespace Business.Services
{
    public class TimesheetService : ResponseMessageService, ITimesheetService
    {
        #region Property
        protected readonly IMapper Mapper;
        protected readonly IUnitOfWork UnitOfWork;
        private readonly HostResource _hostResource;
        private readonly IWebHostEnvironment _env;
        #endregion

        #region Constructor
        public TimesheetService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment env,
            IOptionsMonitor<HostResource> hostResource,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(responseMessage)
        {
            this.Mapper = mapper;
            this.UnitOfWork = unitOfWork;
            this._hostResource = hostResource.CurrentValue;
            this._env = env;
        }
        #endregion

        #region Method
        public async Task<BaseResponse<TimesheetResource>> ImportAsync(Stream stream)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                await package.LoadAsync(stream);

                // Get the first worksheet in the workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                var year = (double?)worksheet.Cells[3, 2].Value;
                var month = (double?)worksheet.Cells[3, 4].Value;

                if (year == null || month == null)
                    return new BaseResponse<TimesheetResource>(ResponseMessage.Values["Timesheet_Invalid"]);

                Console.WriteLine($"Timesheet - Year: {year}");
                Console.WriteLine($"Timesheet - Month: {month}");

                //int col = 2; // Column 2 is the item description
                //for (int row = 2; row < 5; row++)
                //    Console.WriteLine("\tCell({0},{1}).Value={2}", row, col, worksheet.Cells[row, col].Value);

                string newPath = GetRootPath($"{year}-{month}.xlsx");
                if (File.Exists(newPath))
                    File.Delete(newPath);

                await package.SaveAsAsync(newPath);
            }

            return new BaseResponse<TimesheetResource>(true);
        }

        #region Private work
        private string GetRootPath(string timesheetFileName)
        {
            string timesheetPath = string.Format($"{_hostResource.TimesheetPath}{timesheetFileName}");
            string rootPath = string.Concat(_env.WebRootPath, timesheetPath);

            return rootPath;
        }
        #endregion

        #endregion
    }

}
