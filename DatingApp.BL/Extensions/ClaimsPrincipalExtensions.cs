using System.Security.Claims;

namespace DatingApp.BL.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUsername(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Name)?.Value;
    }
    
    public static int? GetUserId(this ClaimsPrincipal user)
    {
        if (Int32.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            return userId;
        else
            return null;
    }
}