namespace CalculadoraSQIA.Domain.Helpers
{
    public static class CalculoHelper
    {
        public static decimal FatorAcumulado(decimal fatorAcumulado, decimal fatorDiario)
        {
            return Math.Round(fatorAcumulado * fatorDiario, 16);
        }

        public static decimal ValorAtualizado(decimal valorInicial, decimal fatorAcumulado)
        {
            return Math.Round(valorInicial * fatorAcumulado, 2);
        }
    }
}
