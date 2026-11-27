namespace ModeloBackEndAPI.Application.Enums
{
    /// <summary>
    /// Define os valores possíveis para <c>ExtratoPeriodo</c>.
    /// </summary>
    public enum ExtratoPeriodo
    {
        /// <summary>
        /// Período referente ao dia.
        /// </summary>
        Dia,
        /// <summary>
        /// Período referente aos últimos sete dias.
        /// </summary>
        Semana,
        /// <summary>
        /// Período referente aos últimos trinta dias.
        /// </summary>
        Mes,
        /// <summary>
        /// Período referente aos últimos 365 dias.
        /// </summary>
        Ano
    }
}