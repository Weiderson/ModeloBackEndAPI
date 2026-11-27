using DesafioBackendAPI.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackendAPI.Infrastructure
{
    public partial class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) { }

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

        public virtual DbSet<Conta> Conta { get; set; }
        public virtual DbSet<Transacao> Transacao { get; set; }

        public virtual DbSet<Usuario> Usuario { get; set; }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    }
}