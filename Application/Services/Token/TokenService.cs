using ModeloBackEndAPI.Domain.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ModeloBackEndAPI.Application.Services.Token
{
    /// <summary>
    /// Representa <c>Token Service</c>.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _Key;

        /// <summary>
        /// Inicializa uma nova instância de <c>Token Service</c>.
        /// </summary>
        public TokenService(IConfiguration configuration)
        {
            var jwtKey = configuration["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
                throw new InvalidOperationException("Configuration 'Jwt:Key' is required.");

            _Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        }


        /// <summary>
        /// Executa a operação <c>Criar Token</c>.
        /// </summary>
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
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = creds
            };

            var handker = new JwtSecurityTokenHandler();

            var token = handker.CreateToken(description);

            return handker.WriteToken(token);
        }
    }
}
