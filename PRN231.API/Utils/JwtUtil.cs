using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PRN231.API.Constants;
using PRN231.Repo.Models;

namespace PRN231.API.Utils;

public class JwtUtil
{
    public static string GenerateJwtToken(Account account)
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables(JwtConstant.JwtEnvironment)
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
        var jwtHandler = new JwtSecurityTokenHandler();
        var secrectKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtConstant:" + JwtConstant.SecretKey]));
        var credentials = new SigningCredentials(secrectKey, SecurityAlgorithms.HmacSha256Signature);
        var issuer = configuration["JwtConstant:" + JwtConstant.Issuer];
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, account.Username),
            new(ClaimTypes.Role, account.Role.Name)
        };
        var expires = DateTime.Now.AddDays(30);
        var token = new JwtSecurityToken(issuer, null, claims, DateTime.Now, expires, credentials);
        return jwtHandler.WriteToken(token);
    }
}