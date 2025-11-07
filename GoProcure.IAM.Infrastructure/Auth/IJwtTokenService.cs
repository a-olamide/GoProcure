using GoProcure.IAM.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.IAM.Infrastructure.Auth
{
    public interface IJwtTokenService
    {
        Task<TokenResponse> IssueAsync(AppUser user, CancellationToken ct = default);
        Task<TokenResponse?> RefreshAsync(string refreshToken, CancellationToken ct = default);
    }
}
