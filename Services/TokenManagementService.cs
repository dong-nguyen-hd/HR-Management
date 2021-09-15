#nullable enable
using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Domain.Services.Communication;
using HR_Management.Resources;
using HR_Management.Resources.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HR_Management.Services
{
    public class TokenManagementService : ResponseMessageService, ITokenManagementService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly byte[] _secret;

        public TokenManagementService(IAccountRepository accountRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOptionsSnapshot<ResponseMessage> responseMessage) : base(responseMessage)
        {
            this._accountRepository = accountRepository;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
            _secret = Encoding.ASCII.GetBytes(Startup.JwtConfig.Secret);
        }

        public async Task<TokenManagementResponse<TokenResource>> GenerateTokensAsync(LoginResource loginResource, DateTime now)
        {
            // Validate Login-Resource
            var tempAccount = await _accountRepository.ValidateCredentialsAsync(loginResource);
            if (tempAccount is null)
                return new TokenManagementResponse<TokenResource>(ResponseMessage.Values["Token_Invalid"]);
            // Get claim value
            Claim[] claims = GetClaim(tempAccount);

            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);

            var jwtToken = new JwtSecurityToken(
                Startup.JwtConfig.Issuer,
                shouldAddAudienceClaim ? Startup.JwtConfig.Audience : string.Empty,
                claims,
                expires: now.AddHours(Startup.JwtConfig.AccessTokenExpirationHours),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            // Set Last-Activity value
            tempAccount.LastActivity = DateTime.Now;

            try
            {
                await _unitOfWork.CompleteAsync();

                // Mapping
                var resource = _mapper.Map<Account, TokenResource>(tempAccount);
                resource.AccessToken = accessToken;

                return new TokenManagementResponse<TokenResource>(resource);
            }
            catch (Exception ex)
            {
                return new TokenManagementResponse<TokenResource>($"{ResponseMessage.Values["Token_Saving_Error"]}: {ex.Message}");
            }
        }

        private static Claim[] GetClaim(Account account)
        {
            var claims = new[]
            {
                // Optional: you can add other Claims.
                // Note: You avoid sensitive information, because this is public.
                new Claim(ClaimTypes.Name, account.UserName),
                new Claim(ClaimTypes.Role, account.Role),
            };

            return claims;
        }
    }
}
