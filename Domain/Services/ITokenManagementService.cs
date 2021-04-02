using HR_Management.Domain.Services.Communication;
using HR_Management.Resources.Authentication;
using System;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface ITokenManagementService
    {
        Task<TokenManagementResponse<TokenResource>> GenerateTokensAsync(LoginResource loginResource, DateTime now);
    }
}
