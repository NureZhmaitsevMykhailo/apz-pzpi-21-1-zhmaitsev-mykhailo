using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Core.Constants;
using API.Services.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace API.Core.Middlewares;

public class JwtMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, IJwtService jwtService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            AttachUserToContext(context, jwtService, token);

        await next(context);
    }

    private void AttachUserToContext(HttpContext context, IJwtService jwtService, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var configurationSecret = jwtService.Configuration["Authorization:JWT:Secret"];
            var key = Encoding.ASCII.GetBytes(configurationSecret);
    
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var isDoctor = Enum.TryParse<Role>(jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role && x.Value == nameof(Role.DoctorRole))?.Value ?? "", out var role);
            
            context.Items["IsDoctor"] = isDoctor;
            context.Items["Id"] = Convert.ToInt32(jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
        }
        catch
        {
            // Do nothing if token validation fails
        }
    }
}
