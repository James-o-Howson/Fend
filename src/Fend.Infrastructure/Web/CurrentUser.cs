using System.Security.Claims;
using Fend.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Fend.Infrastructure.Web;

internal sealed class CurrentUser : IUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
}