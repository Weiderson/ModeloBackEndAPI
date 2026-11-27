using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ModeloBackEndAPI.Application.DTOs
{
    /// <summary>
    /// Representa os dados de <c>Usuario</c> usados pela API.
    /// </summary>
    public class UsuarioDTO
    {

        /// <summary>
        /// Identificador do registro.
        /// </summary>
        [SwaggerIgnore]
        public int? Id { get; set; }

        /// <summary>
        /// Nome do usuário.
        /// </summary>
        [SwaggerSchema(ReadOnly = true)]
        public string Nome { get; set; } = null!;

        /// <summary>
        /// Endereço de e-mail do usuário.
        /// </summary>
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;


        /// <summary>
        /// Senha fornecida para autenticação.
        /// </summary>
        [DataType(DataType.Password)]
        [SwaggerRequestBody]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        [MaxLength(20, ErrorMessage = "A senha deve ter no máximo 20 caracteres.")]
        public string Senha { get; set; } = null!;

        /// <summary>
        /// Data e hora de inclusão do registro.
        /// </summary>
        [SwaggerIgnore]
        [DataType(DataType.DateTime)]
        public DateTime DtInclusao { get; set; }

        /// <summary>
        /// Data e hora de exclusão lógica do registro, se houver.
        /// </summary>
        [SwaggerIgnore]
        [DataType(DataType.DateTime)]
        public DateTime? DtExclusao { get; set; }

        /// <summary>
        /// Data e hora da última alteração do registro, se houver.
        /// </summary>
        [SwaggerIgnore]
        [DataType(DataType.DateTime)]
        public DateTime? DtAlteracao { get; set; }

    }
}
