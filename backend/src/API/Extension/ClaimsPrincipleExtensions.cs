using System.Security.Claims;

namespace API.Extension
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            var username = user.FindFirst(ClaimTypes.GivenName)?.Value ?? user.FindFirst(ClaimTypes.Surname)?.Value;
            if (string.IsNullOrEmpty(username))
            {
                throw new InvalidOperationException("Username cannot be null or empty.");
            }
            return username;
        }
    }
}
