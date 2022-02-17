using Business.Communication;
using Business.Resources.Account;
using System.IO;
using System.Threading.Tasks;

namespace Business.Domain.Services
{
    public interface IImageService
    {
        Task<BaseResponse<AccountResource>> SaveImagePersonAsync(int personId, Stream imageStream);
        Task<BaseResponse<AccountResource>> SaveImageAccountAsync(int accountId, Stream imageStream);
    }
}
