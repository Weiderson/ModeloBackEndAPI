using System.Security.Cryptography;
using System.Text;

namespace ModeloBackEndAPI.Application.Services
{
    /// <summary>
    /// Fornece operações criptográficas relacionadas a senhas.
    /// </summary>
    public static class UsuarioService
    {
        /// <summary>
        /// Gera um hash criptográfico para a senha informada.
        /// </summary>
        public static string GerarHash(string senha)
        {
            using (var hmac = new HMACSHA256())
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));

                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// Gera o salt criptográfico para a senha informada.
        /// </summary>
        public static string GerarSalt(string senha)
        {
            using (var hmac = new HMACSHA256())
            {
                var salt = hmac.Key;
                return Convert.ToBase64String(salt);
            }
        }
    }
}
