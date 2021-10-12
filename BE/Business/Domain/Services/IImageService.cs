using Business.Communication;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Business.Domain.Services
{
    public interface IImageService
    {
        Task<BaseResponse<Uri>> SaveImageAsync(int personId, Stream imageStream);
    }
}
