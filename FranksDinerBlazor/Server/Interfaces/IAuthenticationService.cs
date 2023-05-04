using FranksDinerBlazor.Shared.Dtos;

namespace FranksDinerBlazor.Server.Interfaces
{
    public interface IAuthenticationService
    {
    Task<string> SocialLogin(SocialLoginRequest request);
    }
}
