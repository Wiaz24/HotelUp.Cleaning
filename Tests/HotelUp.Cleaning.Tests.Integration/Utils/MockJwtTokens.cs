using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace HotelUp.Customer.Tests.Integration.Utils;

public static class MockJwtTokens
{
    public static string Issuer { get; } = Guid.NewGuid().ToString();
    public static SecurityKey SecurityKey { get; }
    public static SigningCredentials SigningCredentials { get; }

    private static readonly JwtSecurityTokenHandler SecTokenHandler = new JwtSecurityTokenHandler();
    private static readonly RandomNumberGenerator SecRng = RandomNumberGenerator.Create();
    private static readonly byte[] SecKey = new byte[32];

    static MockJwtTokens()
    {
        SecRng.GetBytes(SecKey);
        SecurityKey = new SymmetricSecurityKey(SecKey) { KeyId = Guid.NewGuid().ToString() };
        SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
    }

    public static string GenerateJwtToken(IEnumerable<Claim> claims)
    {
        return SecTokenHandler.WriteToken(new JwtSecurityToken(Issuer, 
            null, claims, null, DateTime.UtcNow.AddMinutes(20), SigningCredentials));
    }
    
    public static IServiceCollection AddMockJwtTokens(this IServiceCollection services)
    {
        services.Configure<JwtBearerOptions>(
            JwtBearerDefaults.AuthenticationScheme,
            options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Issuer,
                    IssuerSigningKey = SecurityKey
                };
                options.Configuration = new OpenIdConnectConfiguration
                {
                    Issuer = Issuer,
                };
            }
        );
        return services;
    }
}
