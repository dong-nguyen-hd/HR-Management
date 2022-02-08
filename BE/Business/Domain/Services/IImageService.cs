using Business.Communication;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Business.Domain.Services
{
    public interface IImageService
    {
        Task<BaseResponse<Uri>> SaveImagePersonAsync(int personId, Stream imageStream);
        Task<BaseResponse<Uri>> SaveImageAccountAsync(int accountId, Stream imageStream);
    }
}
