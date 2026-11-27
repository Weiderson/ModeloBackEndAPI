using DesafioBackendAPI.Application.Options;
using DesafioBackendAPI.Application.Services.Token;
using DesafioBackendAPI.Domain.Model;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Xunit;

namespace DesafioBackendAPI.Tests;

public class JwtTokenServiceTests
{
    [Fact]
    public void CreatedTokenValidatesIssuerAudienceLifetimeAndSignature()
    {
        const string key = "test-only-signing-key-with-at-least-32-bytes";
        const string issuer = "test-issuer";
        const string audience = "test-audience";
        var service = new TokenService(Options.Create(new JwtOptions
        {
            Key = key,
            Issuer = issuer,
            Audience = audience
        }));

        var token = service.CriarToken(new Usuario { Email = "user@example.test" });
        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        Assert.Equal(issuer, ((JwtSecurityToken)validatedToken).Issuer);
        Assert.Contains(principal.Claims, claim => claim.Value == "user@example.test");
    }
}
