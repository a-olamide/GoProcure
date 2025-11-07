using GoProcure.Api.Dtos;
using GoProcure.IAM.Infrastructure.Auth;
using GoProcure.IAM.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GoProcure.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public sealed class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _users;
        private readonly SignInManager<AppUser> _signIn;
        private readonly IJwtTokenService _jwt;

        public AuthController(UserManager<AppUser> users, SignInManager<AppUser> signIn, IJwtTokenService jwt)
        {
            _users = users; _signIn = signIn; _jwt = jwt;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto, CancellationToken ct)
        {
            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = dto.Email,
                Email = dto.Email,
                //FirstName = dto.FirstName,
                //LastName = dto.LastName,
                //Department = dto.Department,
                EmailConfirmed = true
            };

            var result = await _users.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            var tokens = await _jwt.IssueAsync(user, ct);
            return Ok(tokens);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto, CancellationToken ct)
        {
            var user = await _users.FindByEmailAsync(dto.Email);
            if (user is null) return Unauthorized();

            var check = await _signIn.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!check.Succeeded) return Unauthorized();

            var tokens = await _jwt.IssueAsync(user, ct);
            return Ok(tokens);
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshDto dto, CancellationToken ct)
        {
            var tokens = await _jwt.RefreshAsync(dto.RefreshToken, ct);
            return tokens is null ? Unauthorized() : Ok(tokens);
        }
    }
}
