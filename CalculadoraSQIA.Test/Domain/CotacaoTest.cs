using CalculadoraSQIA.Domain.Entities;

namespace CalculadoraSQIA.Test.Domain
{
    public class CotacaoTest
    {
        [Fact]
        public void DeveRetornarFatorDiario()
        {
            var cotacao = new Cotacao()
            {
                Id = 1,
                Data = DateTime.Now,
                Indexador = "SQI",
                Valor = 1000m
            };

            var resultado = cotacao.CalcularFatorDiario();

            var esperado = 1.00956087m;
            Assert.Equal(esperado, resultado);
        }
    }
}
