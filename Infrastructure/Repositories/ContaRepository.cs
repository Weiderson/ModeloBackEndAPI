using ModeloBackEndAPI.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace ModeloBackEndAPI.Infrastructure.Repositories
{
    /// <summary>
    /// Implementa as operações de persistência de <c>Conta</c>.
    /// </summary>
    public class ContaRepository : IContaRepository
    {
        private readonly Contexto _db;
        private readonly IHttpContextAccessor _httpContextAccessor;


        /// <summary>
        /// Inicializa uma nova instância de <c>Conta Repository</c>.
        /// </summary>
        public ContaRepository(Contexto db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        #region CRUD

        /// <summary>
        /// Adiciona o registro informado de forma assíncrona.
        /// </summary>
        public async Task AddAsync(Conta conta)
        {
            _db.Conta.Add(conta);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Obtém os registros de forma assíncrona.
        /// </summary>
        public async Task<IEnumerable<Conta>> GetAsync()
        {
            var contas = await _db.Conta.ToListAsync().ConfigureAwait(false);
            return contas;
        }

        /// <summary>
        /// Obtém os registros de forma assíncrona.
        /// </summary>
        public async Task<Conta?> GetAsync(int id)
        {
            var conta = await _db.Conta.FirstOrDefaultAsync(f => f.Id == id);
            return conta;
        }

        /// <summary>
        /// Exclui ou marca o registro como excluído, conforme a versão da API.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            string? caminho = httpContext?.Request.Path.ToString();

            if (caminho == null)
                throw new Exception("Caminho não encontrado.");


            var conta = await GetAsync(id).ConfigureAwait(false);
            if (conta is null)
                throw new Exception("Conta inexistente.");

            if (caminho.StartsWith("/api/v1"))
            {
                conta.DtExclusao = DateTime.Now;
                _db.Conta.Update(conta);
            }
            else if (caminho.StartsWith("/api/v2"))
            {
                conta.DtExclusao = DateTime.Now;
                _db.Conta.Remove(conta);
            }

            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Atualiza o registro informado de forma assíncrona.
        /// </summary>
        public async Task UpdateAsync(Conta conta)
        {
            _db.Conta.Update(conta);

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        #endregion CRUD
    }
}