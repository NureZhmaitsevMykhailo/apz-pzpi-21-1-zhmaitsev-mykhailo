using API.Core.Constants;

namespace API.Services.Abstractions;

public interface IJwtService
{
    public IConfiguration Configuration { get; set; }
    public string GenerateToken(int userId, Role role);
}
