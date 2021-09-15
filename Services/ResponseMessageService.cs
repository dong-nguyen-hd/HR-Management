using HR_Management.Resources;
using Microsoft.Extensions.Options;

namespace HR_Management.Services
{
    public abstract class ResponseMessageService
    {
        protected readonly ResponseMessage ResponseMessage;

        public ResponseMessageService(IOptionsSnapshot<ResponseMessage> responseMessage)
        {
            this.ResponseMessage = responseMessage.Value;
        }
    }
}
