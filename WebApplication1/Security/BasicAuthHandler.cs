using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using WebApplication1.Services;
using ISystemClock = Microsoft.AspNetCore.Authentication.ISystemClock;

namespace WebApplication1.Security
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions> 
    {
        private readonly IUserService _userService;

        public BasicAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserService userService
            ) : base(options, logger, encoder, clock )
        {
            _userService = userService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("No vino el header");

            bool result = false;

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialhHeader = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialhHeader).Split(":");
                var email = credentials[0];
                var pass = credentials[1];
                result = _userService.isUser(email, pass);
            }
            catch
            {
                return AuthenticateResult.Fail("Paso algo");
            }
            if (!result)
                return AuthenticateResult.Fail("Usuario o contrasenia invalidos");

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "id"),
                new Claim(ClaimTypes.Name, "user")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);

        }
    }
}
