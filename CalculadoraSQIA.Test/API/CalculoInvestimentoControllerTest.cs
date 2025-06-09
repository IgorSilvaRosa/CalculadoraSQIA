using CalculadoraSQIA.API.Controllers;

using CalculadoraSQIA.Application.UseCases;
using CalculadoraSQIA.Domain.Interfaces;
using CalculadoraSQIA.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CalculadoraSQIA.Test.API
{

    public class CalculoInvestimentoControllerTest
    {
        [Fact]
        public async Task DeveRetornarBadRequestComDataInicioMenorQueDataFinal()
        {
            var service = new Mock<ICalculoService>();
            var logger = new Mock<ILogger<CalculoInvestimentoController>>();
            var controller = new CalculoInvestimentoController(service.Object, logger.Object);

            decimal valorInicial = 1000m;
            DateTime dataAplicacao = new DateTime(2025, 06, 10);
            DateTime dataFinal = new DateTime(2025, 06, 05);

            var resultado = await controller.Calcular(valorInicial, dataAplicacao, dataFinal);

            Assert.NotNull(resultado);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultado);
            Assert.Equal("Data inicial deve ser menor que data final.", badRequestResult.Value);

        }

        [Fact]
        public async Task DeveRetornarOkComDadosValidos()
        {
            var loggerController = new Mock<ILogger<CalculoInvestimentoController>>();
            var loggerService = new Mock<ILogger<CalculoService>>();
            var repository = new Mock<ICotacaoRepository>();

            var resultadoEsperado = new List<ResultadoCalculo>
            {
                new ResultadoCalculo
                {
                    Data = new DateTime(2025, 06, 10),
                    TaxaAnual = 13.15m,
                    FatorDiario = 1.00000000m,
                    FatorAcumulado = 1.00000000m,
                    ValorAtualizado = 1000.00m
                }
            };

            var service = new Mock<ICalculoService>();
            service.Setup(s => s.Executar(It.IsAny<decimal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(resultadoEsperado);

            var controller = new CalculoInvestimentoController(service.Object, loggerController.Object);

            var resultado = await controller.Calcular(1000m, new DateTime(2025, 06, 10), new DateTime(2025, 06, 30));

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var dados = Assert.IsType<List<ResultadoCalculo>>(okResult.Value);
            Assert.Single(dados);
            Assert.Equal(new DateTime(2025, 06, 10), dados[0].Data);

        }

        [Fact]
        public async Task DeveRetornarListResultadoCalculoVazio()
        {
            var loggerController = new Mock<ILogger<CalculoInvestimentoController>>();
            var loggerService = new Mock<ILogger<CalculoService>>();
            var repository = new Mock<ICotacaoRepository>();

            var resultadoEsperado = new List<ResultadoCalculo>();


            var service = new Mock<ICalculoService>();
            service.Setup(s => s.Executar(It.IsAny<decimal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(resultadoEsperado);

            var controller = new CalculoInvestimentoController(service.Object, loggerController.Object);

            var resultado = await controller.Calcular(1000m, new DateTime(2025, 06, 10), new DateTime(2025, 06, 30));

            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var dados = Assert.IsType<List<ResultadoCalculo>>(okResult.Value);
            Assert.Equal(new List<ResultadoCalculo>(), dados);

        }
    }
}
