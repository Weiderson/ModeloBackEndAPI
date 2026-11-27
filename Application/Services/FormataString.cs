using System.Text.RegularExpressions;

namespace ModeloBackEndAPI.Application.Services
{
    /// <summary>
    /// Fornece utilitários para validação de strings.
    /// </summary>
    public static class FormataString
    {
        /// <summary>
        /// Verifica se o texto contém somente números.
        /// </summary>
        public static bool SomenteNumeros(string valor)
        {
            return Regex.IsMatch(valor, @"^\d+$");
        }
    }
}