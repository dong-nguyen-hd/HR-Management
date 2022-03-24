using Business.Communication;
using Business.Resources.Account;
using Business.Resources.Person;
using System.IO;
using System.Threading.Tasks;

namespace Business.Domain.Services
{
    public interface IImageService
    {
        Task<BaseResponse<PersonResource>> SaveImagePersonAsync(int personId, Stream imageStream);
        Task<BaseResponse<AccountResource>> SaveImageAccountAsync(int accountId, Stream imageStream);
        Task<BaseResponse<AccountResource>> ResetAccountAvatarAsync(int id);
        Task<BaseResponse<PersonResource>> ResetPersonAvatarAsync(int id);
    }
}
