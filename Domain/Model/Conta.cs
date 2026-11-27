using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModeloBackEndAPI.Domain.Model
{
    /// <summary>
    /// Representa <c>Conta</c>.
    /// </summary>
    [Table("Contas")]
    public class Conta
    {
        /// <summary>
        /// Identificador do registro.
        /// </summary>
        [Key]
        public int? Id { get; set; }

        /// <summary>
        /// CNPJ associado à conta.
        /// </summary>
        public string Cnpj { get; set; } = null!;

        /// <summary>
        /// Número da conta.
        /// </summary>
        public string Numero { get; set; } = null!;

        /// <summary>
        /// Agência da conta.
        /// </summary>
        public string Agencia { get; set; } = null!;

        /// <summary>
        /// Categoria de relacionamento da conta.
        /// </summary>
        public string Relacionamento { get; set; } = null!;

        /// <summary>
        /// Situação atual da conta.
        /// </summary>
        public string Situacao { get; set; } = null!;

        /// <summary>
        /// Data e hora de inclusão do registro.
        /// </summary>
        public DateTime DtInclusao { get; set; }

        /// <summary>
        /// Data e hora de exclusão lógica do registro, se houver.
        /// </summary>
        public DateTime? DtExclusao { get; set; }

        /// <summary>
        /// Data e hora da última alteração do registro, se houver.
        /// </summary>
        public DateTime? DtAlteracao { get; set; }

        /// <summary>
        /// Caminho do arquivo associado à conta.
        /// </summary>
        public string LocalArquivo { get; set; } = null!;

        /// <summary>
        /// Saldo atual da conta.
        /// </summary>
        public decimal Saldo { get; set; }

        /// <summary>
        /// Nome completo associado ao registro.
        /// </summary>
        [NotMapped]
        public string NomeCompleto { get; set; } = null!;

        /// <summary>
        /// Transações associadas à conta.
        /// </summary>
        public ICollection<Transacao> Transacoes { get; } = new List<Transacao>();
    }
}