using HR_Management.Domain.Services.Communication;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface IImageService
    {
        Task<ImageResponse<Uri>> SaveImage(int personId, Stream imageStream, bool isMobile = false);
    }
}
