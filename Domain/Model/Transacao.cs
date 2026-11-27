using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModeloBackEndAPI.Domain.Model
{
    /// <summary>
    /// Representa <c>Transacao</c>.
    /// </summary>
    [Table("Transacoes")]
    public class Transacao
    {
        /// <summary>
        /// Identificador do registro.
        /// </summary>
        [Key]
        public int? Id { get; set; }

        /// <summary>
        /// Descrição da transação.
        /// </summary>
        public string Descricao { get; set; } = null!;

        /// <summary>
        /// Tipo da transação.
        /// </summary>
        public string Tipo { get; set; } = null!;

        /// <summary>
        /// Valor monetário da transação.
        /// </summary>
        [Precision(18, 2)]
        public decimal Valor { get; set; }

        /// <summary>
        /// Data e hora de inclusão do registro.
        /// </summary>
        public DateTime DtInclusao { get; set; }

        /// <summary>
        /// Data e hora de exclusão lógica do registro, se houver.
        /// </summary>
        [SwaggerIgnore]
        public DateTime? DtExclusao { get; set; }

        /// <summary>
        /// Identificador da conta de origem.
        /// </summary>
        public int ContaId { get; set; }

        /// <summary>
        /// Conta associada à transação.
        /// </summary>
        public Conta Conta { get; set; } = null!;

        /// <summary>
        /// Identificador da conta de destino, quando aplicável.
        /// </summary>
        public int? ContaId_Destino { get; set; }

    }
}