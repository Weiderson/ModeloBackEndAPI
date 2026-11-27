using DesafioBackendAPI.Domain.Model;
using DesafioBackendAPI.Infrastructure;
using DesafioBackendAPI.Infrastructure.Configuration;
using DesafioBackendAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DesafioBackendAPI.Tests;

public class EntityTrackingTests
{
    [Fact]
    public void PersistenceRegistrationDefaultsQueriesToNoTracking()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = "server=localhost;database=test;user id=test;password=test"
            })
            .Build();
        using var services = new ServiceCollection()
            .AddLogging()
            .AddHttpContextAccessor()
            .AddPersistenceServices(configuration)
            .BuildServiceProvider();

        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<Contexto>();

        Assert.Equal(QueryTrackingBehavior.NoTracking, context.ChangeTracker.QueryTrackingBehavior);
    }

    [Fact]
    public async Task UserReadIsNotTrackedButMutationLookupIsTracked()
    {
        var options = new DbContextOptionsBuilder<Contexto>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .Options;
        await using var context = new Contexto(options);
        var user = new Usuario { Nome = "Test user", Email = "tracking@example.test" };
        context.Usuario.Add(user);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var repository = new UsuarioRepository(context, new HttpContextAccessor());
        var readUser = await repository.GetAsync(user.Email);
        Assert.NotNull(readUser);
        Assert.Equal(EntityState.Detached, context.Entry(readUser).State);

        var trackedUser = await repository.GetTrackedAsync(user.Email);
        Assert.NotNull(trackedUser);
        Assert.Equal(EntityState.Unchanged, context.Entry(trackedUser).State);
    }
}
