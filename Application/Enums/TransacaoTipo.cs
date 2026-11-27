namespace ModeloBackEndAPI.Application.Enums
{
    /// <summary>
    /// Define os valores possíveis para <c>TransacaoTipo</c>.
    /// </summary>
    public enum TransacaoTipo
    {
        /// <summary>
        /// Representa uma operação de saque.
        /// </summary>
        Saque,
        /// <summary>
        /// Representa uma operação de depósito.
        /// </summary>
        Deposito,
        /// <summary>
        /// Representa uma transferência entre contas.
        /// </summary>
        Transferencia,
        /// <summary>
        /// Representa uma operação de pagamento.
        /// </summary>
        Pagamento
    }
}