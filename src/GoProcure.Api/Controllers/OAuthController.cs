using GoProcure.IAM.Infrastructure.Auth;
using GoProcure.IAM.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GoProcure.Api.Controllers
{
    [ApiController]
    [Route("api/oauth")]
    public sealed class OAuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _users;
        private readonly IJwtTokenService _tokens;

        public OAuthController(UserManager<AppUser> users, IJwtTokenService tokens)
        { _users = users; _tokens = tokens; }

        [HttpGet("google")]
        public IActionResult GoogleLogin([FromQuery] string returnUrl = "/")
            => Challenge(new AuthenticationProperties { RedirectUri = Url.Action(nameof(GoogleCallback), new { returnUrl }) }, GoogleDefaults.AuthenticationScheme);

        [HttpGet("google/callback")]
        public async Task<IActionResult> GoogleCallback([FromQuery] string returnUrl = "/")
        {
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (!result.Succeeded) return Redirect($"{returnUrl}?error=external_login_failed");

            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email)) return Redirect($"{returnUrl}?error=no_email");

            var user = await _users.FindByEmailAsync(email);
            if (user is null)
            {
                user = new AppUser { Id = Guid.NewGuid(), Email = email, UserName = email, EmailConfirmed = true };
                await _users.CreateAsync(user);
                // optional: assign default role here
            }

            var jwt = await _tokens.IssueAsync(user);
            return Redirect($"{returnUrl}#token={jwt}"); // or return Ok(jwt) for API-only flows
        }

        [HttpGet("facebook")]
        public IActionResult FacebookLogin([FromQuery] string returnUrl = "/")
            => Challenge(new AuthenticationProperties { RedirectUri = Url.Action(nameof(FacebookCallback), new { returnUrl }) }, FacebookDefaults.AuthenticationScheme);

        [HttpGet("facebook/callback")]
        public async Task<IActionResult> FacebookCallback([FromQuery] string returnUrl = "/")
        {
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (!result.Succeeded) return Redirect($"{returnUrl}?error=external_login_failed");

            var email = result.Principal.FindFirstValue(ClaimTypes.Email) ?? result.Principal.FindFirstValue("urn:facebook:email");
            if (string.IsNullOrEmpty(email)) return Redirect($"{returnUrl}?error=no_email");

            var user = await _users.FindByEmailAsync(email) ?? new AppUser { Id = Guid.NewGuid(), Email = email, UserName = email, EmailConfirmed = true };
            if (user.Id == default) await _users.CreateAsync(user);

            var jwt = await _tokens.IssueAsync(user);
            return Redirect($"{returnUrl}#token={jwt}");
        }
    }
}
