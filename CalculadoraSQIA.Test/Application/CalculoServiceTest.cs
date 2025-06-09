using CalculadoraSQIA.Application.UseCases;
using CalculadoraSQIA.Domain.Entities;
using CalculadoraSQIA.Domain.Interfaces;
using CalculadoraSQIA.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace CalculadoraSQIA.Test.Application
{
    public class CalculoServiceTest
    {
        [Fact]
        public async Task DeveRetornarListaDeResultadoCalculoVazia()
        {
            var _cotacaoRepository = new Mock<ICotacaoRepository>();
            var _logger = new Mock<ILogger<CalculoService>>();

            var service = new Mock<ICalculoService>();
            service.Setup(x => x.Executar(It.IsAny<decimal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<ResultadoCalculo>());

            var objService = new CalculoService(_cotacaoRepository.Object, _logger.Object);

            var resultado = await objService.Executar(1000m, new DateTime(2025, 06, 01), new DateTime(2025, 06, 20));

            Assert.Equal(new List<ResultadoCalculo>(), resultado);
        }

        [Fact]
        public async Task DeveIgnorarDiasNaoUteis()
        {
            var _cotacaoRepoMock = new Mock<ICotacaoRepository>();
            var _logger = new Mock<ILogger<CalculoService>>();
            var _service = new CalculoService(_cotacaoRepoMock.Object, _logger.Object);

            var dataAplicacao = new DateTime(2025, 6, 6); // Sexta
            var dataFinal = new DateTime(2025, 6, 9);     // Segunda

            var cotacoes = new List<Cotacao>
            {
                new Cotacao { Data = dataAplicacao, Valor = 13.15m }, // dia útil
                new Cotacao { Data = new DateTime(2025, 6, 8), Valor = 13.15m }, // domingo
                new Cotacao { Data = dataFinal, Valor = 13.15m } // dia útil
            };

            _cotacaoRepoMock
                .Setup(r => r.ObterPorPeriodoAsync(dataAplicacao, dataFinal))
                .ReturnsAsync(cotacoes);

            var resultado = await _service.Executar(1000m, dataAplicacao, dataFinal);

            // Deve ignorar o domingo
            Assert.Equal(2, resultado.Count);
            Assert.All(resultado, r => Assert.NotEqual(new DateTime(2025, 6, 8), r.Data));
        }

        [Fact]
        public async Task DeveCalcularCorretamenteFatoresEValorAtualizado()
        {
            var _cotacaoRepoMock = new Mock<ICotacaoRepository>();
            var _logger = new Mock<ILogger<CalculoService>>();
            var _service = new CalculoService(_cotacaoRepoMock.Object, _logger.Object);

            var dataAplicacao = new DateTime(2025, 6, 6);
            var dataFinal = new DateTime(2025, 6, 9);

            var cotacoes = new List<Cotacao>
            {
                new Cotacao { Data = dataAplicacao, Valor = 13.15m }, // primeiro dia
                new Cotacao { Data = dataFinal, Valor = 13.15m } // próximo dia útil
            };

            _cotacaoRepoMock
                .Setup(r => r.ObterPorPeriodoAsync(dataAplicacao, dataFinal))
                .ReturnsAsync(cotacoes);

            var resultado = await _service.Executar(1000m, dataAplicacao, dataFinal);

            Assert.Equal(2, resultado.Count);

            var primeiro = resultado[0];
            var segundo = resultado[1];

            Assert.Equal(1m, primeiro.FatorDiario);
            Assert.True(segundo.FatorDiario > 1m);
            Assert.True(segundo.ValorAtualizado > 1000m);
        }
    }
}

