using DesafioBackendAPI.Domain.Interfaces;
using DesafioBackendAPI.Domain.Repositories;
using DesafioBackendAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackendAPI.Infrastructure.Configuration;

public static class PersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Connection string 'DefaultConnection' is required.");

        services.AddDbContext<Contexto>(options => options
            .UseMySQL(connectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        services.AddScoped<IContaRepository, ContaRepository>();
        services.AddScoped<ITransacaoRepository, TransacaoRepository>();
        services.AddScoped<IExtratoRepository, ExtratoRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IBaseRepository, BaseRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
