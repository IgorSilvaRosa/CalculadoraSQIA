using System;
using CalculadoraSQIA.Domain.Entities;
using CalculadoraSQIA.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CalculadoraSQIA.Infrastructure.Repositories
{
    public class CotacaoRepository : ICotacaoRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CotacaoRepository> _logger;

        public CotacaoRepository(AppDbContext context, ILogger<CotacaoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Cotacao>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim)
        {
            _logger.LogInformation($"Chamada do metodo ObterPorPeriodoAsync da camada de Infrastructure com os parametros DataInicio: {inicio} e DataFim: {fim}");

            var inicioStr = inicio.Date.ToString("yyyy-MM-dd");
            var fimStr = fim.Date.ToString("yyyy-MM-dd");

            return await _context.Cotacao
                .Where(c => string.Compare(c.Data.ToString(), inicioStr) >= 0 && string.Compare(c.Data.ToString(), fimStr) <= 0)
                .OrderBy(c => c.Data)
                .ToListAsync();
        }
    }
}
