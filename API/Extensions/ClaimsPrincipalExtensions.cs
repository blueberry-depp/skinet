using System.Security.Claims;

namespace API.Extensions;

// Extend the claims principal rather than UserManager.
public static class ClaimsPrincipalExtensions
{
    public static string RetrieveEmailFromClaimsPrincipal(this ClaimsPrincipal user)
    {
        // Get the email.
        return user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
    }
}