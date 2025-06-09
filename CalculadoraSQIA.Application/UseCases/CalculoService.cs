using System.Text.Json;
using CalculadoraSQIA.Application.Helpers;
using CalculadoraSQIA.Domain.Helpers;
using CalculadoraSQIA.Domain.Interfaces;
using CalculadoraSQIA.Domain.Models;
using Microsoft.Extensions.Logging;

namespace CalculadoraSQIA.Application.UseCases
{
    public class CalculoService : ICalculoService
    {
        private readonly ICotacaoRepository _cotacaoRepository;
        private readonly ILogger<CalculoService> _logger;

        public CalculoService(ICotacaoRepository cotacaoRepo, ILogger<CalculoService> logger)
        {
            _cotacaoRepository = cotacaoRepo;
            _logger = logger;
        }

        /// <summary>
        /// Método responsável pelo calculo de Fator Diário, FatorAcumulado e ValorAtualizado
        /// </summary>
        /// <param name="valorInicial">Valor de aporte</param>
        /// <param name="dataAplicacao">Data de inicio do calculo</param>
        /// <param name="dataFinal">Data final do calculo</param>
        /// <returns></returns>
        public async Task<List<ResultadoCalculo>> Executar(decimal valorInicial, DateTime dataAplicacao, DateTime dataFinal)
        {
            _logger.LogInformation($"Iniciando cálculo. ValorInicial: {valorInicial}, DataAplicacao: {dataAplicacao}, DataFinal: {dataFinal}");

            var cotacoes = await _cotacaoRepository.ObterPorPeriodoAsync(dataAplicacao, dataFinal);

            _logger.LogInformation($"Retorno do metodo ObterPorPeriodoAsync: {JsonSerializer.Serialize(cotacoes)}");

            if (cotacoes == null || cotacoes.Count == 0)
            {
                _logger.LogWarning("Nenhuma cotação encontrada entre {DataInicio} e {DataFim}", dataAplicacao, dataFinal);
                return new List<ResultadoCalculo>();
            }

            var resultados = new List<ResultadoCalculo>();

            decimal fatorAcumulado = 1m;
            decimal valorAtualizado = valorInicial;

            foreach (var cotacao in cotacoes)
            {
                if (cotacao.Data == dataAplicacao)
                {
                    resultados.Add(new ResultadoCalculo
                    {
                        Data = cotacao.Data,
                        TaxaAnual = cotacao.Valor,
                        FatorDiario = 1m,
                        FatorAcumulado = fatorAcumulado,
                        ValorAtualizado = valorAtualizado
                    });
                    continue;
                }

                // Ignorar dias que não são úteis
                if (!DiasUteisHelper.EhDiaUtil(cotacao.Data))
                    continue;

                var fatorDiario = cotacao.CalcularFatorDiario();

                fatorAcumulado = CalculoHelper.FatorAcumulado(fatorAcumulado, fatorDiario);

                valorAtualizado = CalculoHelper.ValorAtualizado(valorInicial, fatorAcumulado);

                resultados.Add(new ResultadoCalculo
                {
                    Data = cotacao.Data,
                    TaxaAnual = cotacao.Valor,
                    FatorDiario = fatorDiario,
                    FatorAcumulado = fatorAcumulado,
                    ValorAtualizado = valorAtualizado
                });
            }

            return resultados;
        }
    }
}
