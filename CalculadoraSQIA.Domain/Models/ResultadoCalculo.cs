﻿namespace CalculadoraSQIA.Domain.Models
{
    public class ResultadoCalculo
    {
        public DateTime Data { get; set; }
        public decimal TaxaAnual { get; set; }
        public decimal FatorDiario { get; set; }
        public decimal FatorAcumulado { get; set; }
        public decimal ValorAtualizado { get; set; }
    }
}
