using GoProcure.Application.Abstraction;
using System.Security.Claims;

namespace GoProcure.Api.Auth
{
    public sealed class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _http;
        public CurrentUser(IHttpContextAccessor http) => _http = http;

        public bool IsAuthenticated => _http.HttpContext?.User?.Identity?.IsAuthenticated == true;

        public Guid? UserId
            => Guid.TryParse(_http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                         ?? _http.HttpContext?.User?.FindFirstValue("sub"), out var id)
               ? id : null;

        public string? Department
            => _http.HttpContext?.User?.FindFirstValue("dept") // custom claim
               ?? _http.HttpContext?.User?.FindFirstValue(ClaimTypes.GroupSid); // or wherever you map it
    }
}
