using ModeloBackEndAPI.Application.Enums;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace ModeloBackEndAPI.Application.DTOs
{
    /// <summary>
    /// Representa os dados de <c>Transacao</c> usados pela API.
    /// </summary>
    public class TransacaoDTO
    {

        /// <summary>
        /// Identificador do registro.
        /// </summary>
        [SwaggerIgnore]
        public int? Id { get; set; }

        /// <summary>
        /// Identificador da conta de origem.
        /// </summary>
        public int ContaId { get; set; }

        /// <summary>
        /// Identificador da conta de destino, quando aplicável.
        /// </summary>
        public int? ContaId_Destino { get; set; }

        /// <summary>
        /// Descrição da transação.
        /// </summary>
        public string Descricao { get; set; } = null!;

        /// <summary>
        /// Tipo da transação.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransacaoTipo Tipo { get; set; }

        /// <summary>
        /// Valor monetário da transação.
        /// </summary>
        [Precision(18, 2)]
        public decimal Valor { get; set; }

        /// <summary>
        /// Data e hora de inclusão do registro.
        /// </summary>
        [SwaggerIgnore]
        public DateTime DtInclusao { get; set; }

        /// <summary>
        /// Data e hora de exclusão lógica do registro, se houver.
        /// </summary>
        [SwaggerIgnore]
        public DateTime? DtExclusao { get; set; }
    }
}