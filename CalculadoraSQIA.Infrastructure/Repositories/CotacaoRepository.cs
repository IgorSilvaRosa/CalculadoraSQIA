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

            return await _context.Cotacao
                .Where(c => c.Data >= inicio && c.Data <= fim)
                .OrderBy(c => c.Data)
                .ToListAsync();
        }
    }
}
