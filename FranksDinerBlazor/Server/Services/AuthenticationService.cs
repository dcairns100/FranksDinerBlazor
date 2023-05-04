using FranksDinerBlazor.Server.Interfaces;
using FranksDinerBlazor.Shared.Dtos;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Server;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FranksDinerBlazor.Server.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthenticationService(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> SocialLogin(SocialLoginRequest request)
        {
            var tokenValidationResult = await ValidateSocialToken(request);

            if (tokenValidationResult.IsFailed)
            {
                return tokenValidationResult;
            }

            var token = GetToken(await GetClaims());

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GetErrorsText(IEnumerable<IdentityError> errors)
        {
            return string.Join(", ", errors.Select(error => error.Description).ToArray());
        }

        private async Task ValidateSocialToken(SocialLoginRequest request)
        {
            await ValidateGoogleToken(request);
        }

        private async Task<string> ValidateGoogleToken(SocialLoginRequest request)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string> { _configuration["SocialLogin:Google:TokenAudience"] }
                };
                await GoogleJsonWebSignature.ValidateAsync(request.AccessToken, settings);

            }
            catch (InvalidJwtException _)
            {
                return $"Google access token is not valid.";
            }

            return "";
        }

        private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return token;
        }

        private async Task<List<Claim>> GetClaims()
        {
            var authClaims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            return authClaims;
        }
    }
}
