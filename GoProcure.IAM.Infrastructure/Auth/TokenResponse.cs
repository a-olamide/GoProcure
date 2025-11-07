using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoProcure.IAM.Infrastructure.Auth
{
    public sealed class TokenResponse
    {
        public string AccessToken { get; init; } = default!;
        public int ExpiresIn { get; init; }              // seconds
        public string RefreshToken { get; init; } = default!;
        public string TokenType { get; init; } = "Bearer";
    }
}
