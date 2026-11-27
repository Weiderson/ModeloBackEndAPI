using System.ComponentModel.DataAnnotations.Schema;

namespace ModeloBackEndAPI.Domain.Model
{
    /// <summary>
    /// Representa <c>Usuario</c>.
    /// </summary>
    [Table("Usuarios")]
    public class Usuario
    {
        /// <summary>
        /// Identificador do registro.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Nome do usuário.
        /// </summary>
        public string Nome { get; set; } = null!;

        /// <summary>
        /// Endereço de e-mail do usuário.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Hash criptográfico da senha do usuário.
        /// </summary>
        public byte[]? SenhaHash { get; set; }

        /// <summary>
        /// Salt criptográfico usado no hash da senha.
        /// </summary>
        public byte[]? SenhaSalt { get; set; }

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
    }
}
