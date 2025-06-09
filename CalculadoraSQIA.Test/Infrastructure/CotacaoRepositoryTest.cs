using CalculadoraSQIA.Domain.Entities;
using CalculadoraSQIA.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace CalculadoraSQIA.Test.Infrastructure
{
    public class CotacaoRepositoryTest
    {
        [Fact]
        public async Task DeveRetornarUmaListaDeCotacao()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("app.db")
            .Options;

            var logger = new Mock<ILogger<CotacaoRepository>>();

            using var context = new AppDbContext(options);

            context.Cotacao.AddRange(
                new Cotacao { Data = new DateTime(2025, 01, 01), Valor = 12.5m },
                new Cotacao { Data = new DateTime(2025, 01, 02), Valor = 13.0m }
            );
            await context.SaveChangesAsync();

            var repository = new CotacaoRepository(context, logger.Object);

            // Act
            var resultado = await repository.ObterPorPeriodoAsync(
                new DateTime(2025, 01, 01),
                new DateTime(2025, 01, 01)
            );

            // Assert
            Assert.Single(resultado);
            Assert.Equal(12.5m, resultado.First().Valor);
        }
    }
}
