namespace ModeloBackEndAPI.Application.ViewModel
{
    /// <summary>
    /// Representa os dados retornados pela consulta à ReceitaWS.
    /// </summary>
    public class ReceitaCnpjVM
    {
        /// <summary>
        /// Campo <c>Cnpj</c> recebido na resposta de consulta à ReceitaWS.
        /// </summary>
        public string? Cnpj { get; set; }
        /// <summary>
        /// Campo <c>Nome</c> recebido na resposta de consulta à ReceitaWS.
        /// </summary>
        public string? Nome { get; set; }
        /// <summary>
        /// Campo <c>Situacao</c> recebido na resposta de consulta à ReceitaWS.
        /// </summary>
        public string? Situacao { get; set; }
        /// <summary>
        /// Campo <c>DataSituacao</c> recebido na resposta de consulta à ReceitaWS.
        /// </summary>
        public string? DataSituacao { get; set; }
        /// <summary>
        /// Campo <c>MotivoSituacao</c> recebido na resposta de consulta à ReceitaWS.
        /// </summary>
        public string? MotivoSituacao { get; set; }
        /// <summary>
        /// Campo <c>NomeFantasia</c> recebido na resposta de consulta à ReceitaWS.
        /// </summary>
        public string? NomeFantasia { get; set; }
        /// <summary>
        /// Campo <c>DataAbertura</c> recebido na resposta de consulta à ReceitaWS.
        /// </summary>
        public string? DataAbertura { get; set; }
        /// <summary>
        /// Campo <c>CapitalSocial</c> recebido na resposta de consulta à ReceitaWS.
        /// </summary>
        public string? CapitalSocial { get; set; }
        /// <summary>
        /// Campo <c>CnaePrincipal</c> recebido na resposta de consulta à ReceitaWS.
        /// </summary>
        public string? CnaePrincipal { get; set; }
        /// <summary>
        /// Campo <c>CnaeSecundario</c> recebido na resposta de consulta à ReceitaWS.
        /// </summary>
        public string? CnaeSecundario { get; set; }
        /// <summary>
        /// Campo <c>TipoLogradouro</c> recebido na resposta de consulta à ReceitaWS.
        /// </summary>
        public string? TipoLogradouro { get; set; }
        /// <summary>
        /// Campo <c>Logradouro</c> recebido na resposta de consulta à ReceitaWS.
        /// </summary>
        public string? Logradouro { get; set; }
        /// <summary>
        /// Campo <c>Numero</c> recebido na resposta de consulta à ReceitaWS.
        /// </summary>
        public string? Numero { get; set; }
        /// <summary>
        /// Campo <c>Complemento</c> recebido na resposta de consulta à ReceitaWS.
        /// </summary>
        public string? Complemento { get; set; }
        /// <summary>
        /// Campo <c>Bairro</c> recebido na resposta de consulta à ReceitaWS.
        /// </summary>
        public string? Bairro { get; set; }
        /// <summary>
        /// Campo <c>Cep</c> recebido na resposta de consulta à ReceitaWS.
        /// </summary>
        public string? Cep { get; set; }
        /// <summary>
        /// Campo <c>Uf</c> recebido na resposta de consulta à ReceitaWS.
        /// </summary>
        public string? Uf { get; set; }
        /// <summary>
        /// Campo <c>Municipio</c> recebido na resposta de consulta à ReceitaWS.
        /// </summary>
        public string? Municipio { get; set; }
    }
}