using System.Diagnostics;

namespace RDS.Api.Common.Api;

public static class RoleClaimsExtension
{
    public static IEnumerable<Claim> GetClaims(this User user)
    {
        var result = new List<Claim>
        {
            new(ClaimTypes.Name, user.Id.ToString())
        };
        if (user.Roles != null)
            result.AddRange(
                user.Roles.Select(role =>
                {
                    Debug.Assert(role.NormalizedName != null, "role.NormalizedName != null");
                    return new Claim(ClaimTypes.Role, role.NormalizedName);
                }));
        return result;
        
        // if (user.Roles != null)
        //     result.AddRange(
        //         user.Roles.Select(role =>
        //         {
        //             Debug.Assert(role.NormalizedName != null, "role.NormalizedName != null");
        //             return new Claim(ClaimTypes.Role, role.NormalizedName);
        //         }));
        // return result;
    }
}