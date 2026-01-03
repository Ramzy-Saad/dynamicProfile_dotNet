using System;
using System.Security.Claims;

namespace RunGroupWebApp;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserID(this ClaimsPrincipal user)
    {
        return user?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
