using HR_Management.Domain.Models;

namespace HR_Management.Domain.Services.Communication
{
    public class WorkHistoryResponse : BaseResponse<WorkHistory>
    {
        public WorkHistoryResponse(bool isSuccess) : base(isSuccess) { }

        public WorkHistoryResponse(WorkHistory workHistory) : base(workHistory) { }

        public WorkHistoryResponse(object objWorkHistory) : base(objWorkHistory) { }

        public WorkHistoryResponse(string message) : base(message) { }
    }
}
