namespace API.Core.Entities;

public class JwtOptions
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
}