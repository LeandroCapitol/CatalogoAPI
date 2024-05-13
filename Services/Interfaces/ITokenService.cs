using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APICatalogo.Services.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken GenereteAcessToken(IEnumerable<Claim> claims,
                                            IConfiguration _config);

        string GenereteRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token,
                                                    IConfiguration _config);

    }
}
