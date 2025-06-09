using CalculadoraSQIA.Domain.Models;

namespace CalculadoraSQIA.Domain.Interfaces
{
    public interface ICalculoService
    {
        Task<List<ResultadoCalculo>> Executar(decimal valorInicial, DateTime dataAplicacao, DateTime dataFinal);
    }
}
