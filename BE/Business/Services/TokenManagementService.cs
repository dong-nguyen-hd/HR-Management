using AutoMapper;
using Business.Communication;
using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Authentication;
using Business.Resources.Information;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class TokenManagementService : ResponseMessageService, ITokenManagementService
    {
        #region Property
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly byte[] _secret;
        private readonly JwtConfig _jwtConfig;
        #endregion

        #region Constructor
        public TokenManagementService(IAccountRepository accountRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOptionsMonitor<JwtConfig> jwtConfig,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(responseMessage)
        {
            this._accountRepository = accountRepository;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
            this._jwtConfig = jwtConfig.CurrentValue;
            _secret = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
        }
        #endregion

        #region Method
        public async Task<BaseResponse<TokenResource>> GenerateTokensAsync(LoginResource loginResource, DateTime now)
        {
            // Validate Login-Resource
            var tempAccount = await _accountRepository.ValidateCredentialsAsync(loginResource);
            if (tempAccount is null)
                return new BaseResponse<TokenResource>(ResponseMessage.Values["Token_Invalid"]);
            // Get claim value
            Claim[] claims = GetClaim(tempAccount);

            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);

            var jwtToken = new JwtSecurityToken(
                _jwtConfig.Issuer,
                shouldAddAudienceClaim ? _jwtConfig.Audience : string.Empty,
                claims,
                expires: now.AddHours(_jwtConfig.AccessTokenExpirationHours),
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

                return new BaseResponse<TokenResource>(resource);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BaseResponse<TokenResource>(ResponseMessage.Values["Token_Saving_Error"]);
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
        #endregion
    }
}
