using CalculadoraSQIA.Domain.Entities;

namespace CalculadoraSQIA.Domain.Interfaces
{
    public interface ICotacaoRepository
    {
        Task<List<Cotacao>> ObterPorPeriodoAsync(DateTime inicio, DateTime fim);
    }
}
