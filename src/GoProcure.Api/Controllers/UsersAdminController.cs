
using GoProcure.Api.Dtos;
using GoProcure.IAM.Infrastructure.Auth;
using GoProcure.IAM.Infrastructure.Identity;
using GoProcure.IAM.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoProcure.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin/users")]
    public class UsersAdminController : ControllerBase
    {
        private readonly UserManager<AppUser> _users;
        private readonly IJwtTokenService _jwt;
        private readonly IdentityDb _db;

        public UsersAdminController(UserManager<AppUser> users, IJwtTokenService jwt, IdentityDb db)
        {
            _users = users;
            _jwt = jwt;
            _db = db;
        }

        [HttpPost("{userId:guid}/roles/{role}")]
        public async Task<IActionResult> AddRole(Guid userId, string role)
        {
            var user = await _users.FindByIdAsync(userId.ToString());
            if (user is null) return NotFound();
            var result = await _users.AddToRoleAsync(user, role);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }

        [HttpDelete("{userId:guid}/roles/{role}")]
        public async Task<IActionResult> RemoveRole(Guid userId, string role)
        {
            var user = await _users.FindByIdAsync(userId.ToString());
            if (user is null) return NotFound();
            var result = await _users.RemoveFromRoleAsync(user, role);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("api/admin/users/{userId:guid}/department")]
        public async Task<IActionResult> SetDepartment(Guid userId, [FromBody] SetDepartmentRequest req, CancellationToken ct)
        {
            if (req is null || string.IsNullOrWhiteSpace(req.Department))
                return BadRequest("Department is required.");

            var user = await _users.FindByIdAsync(userId.ToString());
            if (user is null)
                return NotFound($"User with ID {userId} not found.");

            var isValid = await _db.Departments.AnyAsync(d => d.Name == req.Department && d.IsActive, ct);
            if (!isValid)
                return BadRequest($"Invalid department: {req.Department}");

            user.Department = req.Department.Trim();
            await _users.UpdateAsync(user);

            user.Department = req.Department.Trim();

            var result = await _users.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new
            {
                message = $"Department updated successfully for {user.Email}",
                newDepartment = user.Department
            });
        }
    }
}
