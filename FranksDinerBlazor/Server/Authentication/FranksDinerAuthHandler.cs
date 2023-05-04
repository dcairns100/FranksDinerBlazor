using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace FranksDinerBlazor.Server.Authentication;

public class FranksDinerAuthHandler : AuthenticationHandler<FranksDinerAuthSchemeOptions>
{
    public FranksDinerAuthHandler(
        IOptionsMonitor<FranksDinerAuthSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        TokenModel model;

        // validation comes in here
        if (!Request.Headers.ContainsKey("Bearer"))
        {
            return Task.FromResult(AuthenticateResult.Fail("Header Not Found."));
        }

        var header = Request.Headers["Bearer"].ToString();

        if (header == "1234")
        {

                // create claims array from the model
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Name, "DemoTerminal")
                };

                // generate claimsIdentity on the name of the class
                var claimsIdentity = new ClaimsIdentity(claims,
                    nameof(FranksDinerAuthHandler));
                
                // generate AuthenticationTicket from the Identity
                // and current authentication scheme
                var ticket = new AuthenticationTicket(
                    new ClaimsPrincipal(claimsIdentity), this.Scheme.Name);

                // pass on the ticket to the middleware
                return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        
        return Task.FromResult(AuthenticateResult.Fail("Model is Empty"));
    }

    public class TokenModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
    }
}