using DesafioBackendAPI.Application.Options;
using DesafioBackendAPI.Application.Interfaces;
using DesafioBackendAPI.Domain.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DesafioBackendAPI.Application.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _Key;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenService(IOptions<JwtOptions> options)
        {
            _Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key));
            _issuer = options.Value.Issuer;
            _audience = options.Value.Audience;
        }

        public string CriarToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email)
            };

            var creds = new SigningCredentials(_Key, SecurityAlgorithms.HmacSha256);

            var description = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _issuer,
                Audience = _audience,
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = creds
            };

            var handker = new JwtSecurityTokenHandler();

            var token = handker.CreateToken(description);

            return handker.WriteToken(token);
        }
    }
}
