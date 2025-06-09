namespace CalculadoraSQIA.Domain.Entities
{
    public class Cotacao
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string? Indexador { get; set; }
        public decimal Valor { get; set; }

        public decimal CalcularFatorDiario()
        {
            var fatorDiarioDouble = Math.Pow((double)(1m + Valor / 100m), 1.0 / 252.0);

            var fatorDiario = Math.Round((decimal)fatorDiarioDouble, 8);

            return fatorDiario;
        }


    }
}
