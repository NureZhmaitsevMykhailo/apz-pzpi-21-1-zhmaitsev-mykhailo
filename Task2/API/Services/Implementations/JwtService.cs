using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Core.Constants;
using API.Services.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace API.Services.Implementations;

public class JwtService(IConfiguration configuration) : IJwtService
{
    public IConfiguration Configuration { get; set; } = configuration;

    public string GenerateToken(int userId, Role role)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Role, role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("Authorization:JWT:Secret").Value));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
}