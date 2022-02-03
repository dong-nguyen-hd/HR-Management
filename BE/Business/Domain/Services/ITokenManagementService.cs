using Business.Communication;
using Business.Resources.Authentication;
using System;
using System.Threading.Tasks;

namespace Business.Domain.Services
{
    public interface ITokenManagementService
    {
        Task<BaseResponse<AccessTokenResource>> GenerateTokensAsync(LoginResource loginResource, DateTime now, string userAgent);
    }
}
