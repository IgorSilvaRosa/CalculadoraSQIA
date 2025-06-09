using CalculadoraSQIA.Domain.Helpers;

namespace CalculadoraSQIA.Test.Domain
{
    public class CalculoHelpersTest
    {
        [Theory]
        [InlineData(1.000, 1.000, 1.000000)]
        [InlineData(1.00000000, 1.00956087, 1.00956087)]
        public void FatorAcumuladoDeveCalcularCorretamente(decimal fatorAcumulado, decimal fatorDiario, decimal esperado)
        {
            var resultado = CalculoHelper.FatorAcumulado(fatorAcumulado, fatorDiario);

            Assert.Equal(esperado, resultado);
        }

        [Theory]
        [InlineData(1000, 1.00000000, 1000.00)]
        [InlineData(1000, 1.00956087, 1009.56)]
        [InlineData(1234.56, 1.11111111, 1371.73)]
        [InlineData(999.99, 0.99999999, 999.99)]
        public void ValorAtualizadoDeveCalcularCorretamente(decimal valorInicial, decimal fatorAcumulado, decimal esperado)
        {
            // Act
            var resultado = CalculoHelper.ValorAtualizado(valorInicial, fatorAcumulado);

            // Assert
            Assert.Equal(esperado, resultado);
        }
    }
}
