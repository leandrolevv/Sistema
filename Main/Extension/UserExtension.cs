using System.Security.Claims;
using Main.Models;

namespace Main.Extension
{
    public static class UserExtension
    {
        public static IList<Claim> GetClaims(this User user)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.Email));
            claims.AddRange(user.Roles.Select(x => new Claim(ClaimTypes.Role, x.Slug)));

            return claims;
        }
    }
}
