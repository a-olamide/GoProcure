using GoProcure.IAM.Infrastructure.Identity;
using GoProcure.IAM.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.IAM.Infrastructure.Auth
{
    public sealed class JwtTokenService : IJwtTokenService
    {
        private readonly UserManager<AppUser> _users;
        private readonly IdentityDb _db;
        private readonly JwtOptions _opt;
        public JwtTokenService(UserManager<AppUser> users, IOptions<JwtOptions> opt, IdentityDb db)
        { _users = users; _db = db; _opt = opt.Value; }

        
        public async Task<TokenResponse> IssueAsync(AppUser user, CancellationToken ct = default)
        {
            var access = CreateAccessToken(user); // your existing method building JWT
            var refresh = CreateSecureRandomToken(); // 64+ bytes base64

            var rt = new RefreshToken
            {
                UserId = user.Id,
                Token = refresh,
                ExpiresUtc = DateTime.UtcNow.AddDays(_opt.RefreshTokenDays)
            };
            _db.RefreshTokens.Add(rt);
            await _db.SaveChangesAsync(ct);

            return new TokenResponse
            {
                AccessToken = access.Token,
                ExpiresIn = access.ExpiresInSeconds,
                RefreshToken = refresh
            };
        }

        public async Task<TokenResponse?> RefreshAsync(string refreshToken, CancellationToken ct = default)
        {
            var rt = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken, ct);
            if (rt is null || !rt.IsActive) return null;

            var user = await _users.FindByIdAsync(rt.UserId.ToString());
            if (user is null) return null;

            // Rotate refresh token
            rt.RevokedUtc = DateTime.UtcNow;
            var newRefresh = CreateSecureRandomToken();
            var newRt = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefresh,
                ExpiresUtc = DateTime.UtcNow.AddDays(_opt.RefreshTokenDays)
            };
            rt.ReplacedByToken = newRefresh;
            _db.RefreshTokens.Add(newRt);

            var access = CreateAccessToken(user);
            await _db.SaveChangesAsync(ct);

            return new TokenResponse
            {
                AccessToken = access.Token,
                ExpiresIn = access.ExpiresInSeconds,
                RefreshToken = newRefresh
            };
        }
        private (string Token, int ExpiresInSeconds) CreateAccessToken(AppUser user)
        {
            // Create the claims for the user
            var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new(ClaimTypes.Name, user.UserName ?? string.Empty)
    };

            // Add roles as claims
            var roles = _users.GetRolesAsync(user).Result;
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            claims.Add(new Claim("dept", user.Department ?? "UNKNOWN"));
            claims.Add(new Claim("fullname", $"{user.FirstName} {user.LastName}".Trim()));
            // Optional: Add department or other profile claims later
            // claims.Add(new Claim("dept", user.Department ?? "UNKNOWN"));

            // Create signing key and credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.SigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Set expiry
            var expires = DateTime.UtcNow.AddMinutes(_opt.AccessTokenMinutes);

            // Create the token
            var jwt = new JwtSecurityToken(
                issuer: _opt.Issuer,
                audience: _opt.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            // Serialize the token
            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);

            // Return both token and expiry in seconds (for the TokenResponse)
            return (tokenString, (int)TimeSpan.FromMinutes(_opt.AccessTokenMinutes).TotalSeconds);
        }
        // helpers (pseudo)
        private static string CreateSecureRandomToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(bytes);
        }
    }
}
