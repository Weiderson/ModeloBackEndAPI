using ModeloBackEndAPI.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace ModeloBackEndAPI.Infrastructure
{
    /// <summary>
    /// Representa o contexto de acesso aos dados da aplicação.
    /// </summary>
    public partial class Contexto : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        /// <summary>
        /// Inicializa uma nova instância de <c>Contexto</c>.
        /// </summary>
        public Contexto(DbContextOptions<Contexto> options, IConfiguration configuratation)
            : base(options)
        {
            _configuration = configuratation;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        /// <summary>
        /// Configura o provedor e as opções do contexto do Entity Framework.
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured && !string.IsNullOrWhiteSpace(_connectionString))
            {
                optionsBuilder.UseMySQL(_connectionString)
                    .EnableDetailedErrors();
            }
        }

        /// <summary>
        /// Configura o modelo de entidades do Entity Framework.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Conta>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Transacao>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<Conta>()
                .HasMany(e => e.Transacoes)
                .WithOne(e => e.Conta)
                .HasForeignKey(e => e.ContaId)
                .IsRequired();

            OnModelCreatingPartial(modelBuilder);
        }

        /// <summary>
        /// Conjunto de contas armazenadas no contexto.
        /// </summary>
        public virtual DbSet<Conta> Conta { get; set; }
        /// <summary>
        /// Conjunto de transações armazenadas no contexto.
        /// </summary>
        public virtual DbSet<Transacao> Transacao { get; set; }

        /// <summary>
        /// Conjunto de usuários armazenados no contexto.
        /// </summary>
        public virtual DbSet<Usuario> Usuario { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    }
}