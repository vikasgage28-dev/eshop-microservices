using Identity.Core.Features.Auth.Commands.Login;
using MediatR;

namespace Identity.Core.Features.Auth.Commands.SocialLogin
{
    public class SocialLoginCommand : IRequest<LoginResult>
    {
        /// <summary>Provider name — "auth0", "google", "github" etc.</summary>
        public string Provider    { get; set; } = "auth0";

        /// <summary>Access token obtained by the React SPA from the provider.</summary>
        public string AccessToken { get; set; } = string.Empty;
    }
}
