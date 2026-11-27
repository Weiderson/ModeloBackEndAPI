using ModeloBackEndAPI.Application.Enums;
using System.Text.Json.Serialization;

namespace ModeloBackEndAPI.Application.DTOs
{
    /// <summary>
    /// Representa os dados de <c>Extrato</c> usados pela API.
    /// </summary>
    public class ExtratoDTO
    {
        /// <summary>
        /// Identificador da conta de origem.
        /// </summary>
        public int ContaId { get; set; }

        /// <summary>
        /// Período solicitado para a consulta do extrato.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ExtratoPeriodo ExtratoPeriodo { get; set; }

    }
}