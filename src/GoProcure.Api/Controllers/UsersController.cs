using GoProcure.Api.Dtos;
using GoProcure.IAM.Infrastructure.Auth;
using GoProcure.IAM.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GoProcure.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public sealed class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _users;
        private readonly IJwtTokenService _jwt;

        public UsersController(UserManager<AppUser> users, IJwtTokenService jwt)
        {
            _users = users; _jwt = jwt;
        }

        // PUT /api/users/me
        [Authorize]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateMyProfileDto dto, CancellationToken ct)
        {
            // Get current user from claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                       ?? User.FindFirstValue(ClaimTypes.Name); // fallback if needed
            if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

            var user = await _users.FindByIdAsync(userId);
            if (user is null) return Unauthorized();

            // Update fields
            user.FirstName = dto.FirstName.Trim();
            user.LastName = dto.LastName.Trim();
            user.Department = dto.Department.Trim();
            user.JobTitle = dto.JobTitle?.Trim();
            user.DateOfBirth = dto.DateOfBirth;

            var result = await _users.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            // Optional: immediately return a new access token reflecting updated claims (dept/fullname)
            var tokens = await _jwt.IssueAsync(user, ct);

            return Ok(new
            {
                message = "Profile updated",
                access_token = tokens.AccessToken,
                expires_in = tokens.ExpiresIn,
                refresh_token = tokens.RefreshToken
            });
        }

        // GET /api/users/me (handy for the client to fetch current profile)
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var user = await _users.GetUserAsync(User);
            if (user is null) return Unauthorized();

            return Ok(new
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Department,
                user.JobTitle,
                user.DateOfBirth
            });
        }
    }
}
