using ModeloBackEndAPI.Domain.Model;
using ModeloBackEndAPI.Infrastructure;
using ModeloBackEndAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ModeloBackEndAPI.Tests;

public class ExtratoRepositoryTests
{
    [Fact]
    public async Task GetExtratoAsync_IncludesRecentOutgoingAndIncomingTransactionsOnly()
    {
        var options = new DbContextOptionsBuilder<Contexto>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var contexto = new Contexto(options, new ConfigurationBuilder().Build());
        var contaConsultada = new Conta
        {
            Id = 100,
            Agencia = "0001",
            Cnpj = "00000000000000",
            LocalArquivo = "conta.pdf",
            Numero = "100",
            Relacionamento = "Cliente",
            Situacao = "Ativa"
        };
        var contaOrigem = new Conta
        {
            Id = 200,
            Agencia = "0001",
            Cnpj = "00000000000000",
            LocalArquivo = "conta.pdf",
            Numero = "200",
            Relacionamento = "Cliente",
            Situacao = "Ativa"
        };
        contexto.Conta.AddRange(contaConsultada, contaOrigem);

        var agora = DateTime.Now;
        contexto.Transacao.AddRange(
            new Transacao
            {
                Id = 1,
                Descricao = "Saída recente",
                Tipo = "Saque",
                Valor = 10,
                DtInclusao = agora.AddDays(-1),
                ContaId = 100,
                Conta = contaConsultada
            },
            new Transacao
            {
                Id = 2,
                Descricao = "Transferência recebida recente",
                Tipo = "Transferencia",
                Valor = 20,
                DtInclusao = agora.AddDays(-2),
                ContaId = 200,
                Conta = contaOrigem,
                ContaId_Destino = 100
            },
            new Transacao
            {
                Id = 3,
                Descricao = "Saída antiga",
                Tipo = "Saque",
                Valor = 30,
                DtInclusao = agora.AddDays(-8),
                ContaId = 100,
                Conta = contaConsultada
            },
            new Transacao
            {
                Id = 4,
                Descricao = "Transferência recebida antiga",
                Tipo = "Transferencia",
                Valor = 40,
                DtInclusao = agora.AddDays(-8),
                ContaId = 200,
                Conta = contaOrigem,
                ContaId_Destino = 100
            },
            new Transacao
            {
                Id = 5,
                Descricao = "Transação de outra conta",
                Tipo = "Deposito",
                Valor = 50,
                DtInclusao = agora.AddDays(-1),
                ContaId = 200,
                Conta = contaOrigem,
                ContaId_Destino = 300
            });
        await contexto.SaveChangesAsync();

        var repository = new ExtratoRepository(contexto);
        var transacoes = await repository.GetExtratoAsync(100, 7);

        Assert.Equal(new int?[] { 1, 2 }, transacoes.Select(transacao => transacao.Id));
    }
}
