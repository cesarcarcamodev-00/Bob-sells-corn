using System.Security.Claims;

namespace Bob_sells_corn.Extensions;

public static class ClaimsPrincipalExtension
{
    public static Guid GetClientId(this ClaimsPrincipal principal)
    {
        var claim = principal.FindFirst(ClaimTypes.NameIdentifier);

        if (claim == null || !Guid.TryParse(claim.Value, out Guid clientId))
        {
            throw new UnauthorizedAccessException("Client ID not found in token");
        }

        return clientId;
    }
}
