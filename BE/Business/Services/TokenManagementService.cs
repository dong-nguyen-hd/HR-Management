﻿using AutoMapper;
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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class TokenManagementService : ResponseMessageService, ITokenManagementService
    {
        #region Property
        private readonly IAccountRepository _accountRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly byte[] _secret;
        private readonly JwtConfig _jwtConfig;
        #endregion

        #region Constructor
        public TokenManagementService(IAccountRepository accountRepository,
            ITokenRepository tokenRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOptionsMonitor<JwtConfig> jwtConfig,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(responseMessage)
        {
            this._accountRepository = accountRepository;
            this._tokenRepository = tokenRepository;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
            this._jwtConfig = jwtConfig.CurrentValue;
            this._secret = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
        }
        #endregion

        #region Method
        public async Task<BaseResponse<AccessTokenResource>> GenerateTokensAsync(LoginResource loginResource, DateTime now, string userAgent)
        {
            // Validate Login-Resource
            var tempAccount = await _accountRepository.ValidateCredentialsAsync(loginResource);
            if (tempAccount is null)
                return new BaseResponse<AccessTokenResource>(ResponseMessage.Values["Token_Invalid"]);

            // Get access-token
            var accessToken = GenerateAccessToken(tempAccount, now);

            // Get refresh-token
            var refreshToken = GenerateRefreshToken(now, userAgent);

            // Set Last-Activity value
            tempAccount.LastActivity = DateTime.UtcNow;

            try
            {
                tempAccount.Tokens.Add(refreshToken);
                await _unitOfWork.CompleteAsync();

                return new BaseResponse<AccessTokenResource>(MappingTokenResoure(tempAccount, refreshToken, accessToken));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BaseResponse<AccessTokenResource>(ResponseMessage.Values["Token_Saving_Error"]);
            }
        }

        private AccessTokenResource MappingTokenResoure(Account account, Token token, string accessToken)
        {
            var tokenResource = _mapper.Map<Account, AccessTokenResource>(account);
            tokenResource.TokenResource = _mapper.Map<Token, TokenResource>(token);
            tokenResource.AccessToken = accessToken;

            return tokenResource;
        }

        private string GenerateAccessToken(Account account, DateTime now)
        {
            // Get claim value
            Claim[] claims = GetClaim(account);

            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);

            var jwtToken = new JwtSecurityToken(
                _jwtConfig.Issuer,
                shouldAddAudienceClaim ? _jwtConfig.Audience : string.Empty,
                claims,
                expires: now.AddMinutes(_jwtConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return accessToken;
        }

        private static Claim[] GetClaim(Account account)
        {
            var claims = new[]
            {
                // Optional: you can add other Claims.
                // Note: You avoid sensitive information, because this is public.
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.UserName),
                new Claim(ClaimTypes.Role, account.Role),
            };

            return claims;
        }

        private Token GenerateRefreshToken(DateTime now, string userAgent)
            => new()
            {
                RefreshToken = GenerateRefreshTokenString(),
                ExpireTime = now.AddMinutes(_jwtConfig.RefreshTokenExpiration),
                UserAgent = userAgent,
                IsUse = false
            };

        private static string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];

            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
        #endregion
    }
}
