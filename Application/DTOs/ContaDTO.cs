using ModeloBackEndAPI.Application.Enums;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;
using SwaggerIgnoreAttribute = Swashbuckle.AspNetCore.Annotations.SwaggerIgnoreAttribute;

namespace ModeloBackEndAPI.Application.DTOs
{
    /// <summary>
    /// Representa os dados de <c>Conta</c> usados pela API.
    /// </summary>
    public class ContaDTO
    {
        //[SwaggerSchema(ReadOnly = true)]
        //[JsonIgnore]
        //[SwaggerIgnore]
        /// <summary>
        /// Identificador do registro.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Nome completo associado ao registro.
        /// </summary>
        [SwaggerSchema(ReadOnly = true)]
        [SwaggerIgnore]
        public string? NomeCompleto { get; set; }

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
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Relacionamento Relacionamento { get; set; }

        /// <summary>
        /// Situação atual da conta.
        /// </summary>
        [SwaggerSchema(ReadOnly = true)]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Situacao Situacao { get; set; } = new Situacao();

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

        /// <summary>
        /// Data e hora da última alteração do registro, se houver.
        /// </summary>
        [SwaggerIgnore]
        public DateTime? DtAlteracao { get; set; }

        /// <summary>
        /// Arquivo enviado para a conta.
        /// </summary>
        [JsonIgnore]
        public IFormFile Arquivo { get; set; } = null!;

        /// <summary>
        /// Caminho do arquivo associado à conta.
        /// </summary>
        [SwaggerSchema(ReadOnly = true)]
        [SwaggerIgnore]
        public string? LocalArquivo { get; set; }

        /// <summary>
        /// Saldo atual da conta.
        /// </summary>
        [SwaggerSchema(ReadOnly = true)]
        [SwaggerIgnore]
        [Precision(18, 2)]
        public decimal Saldo { get; set; }
    }
}