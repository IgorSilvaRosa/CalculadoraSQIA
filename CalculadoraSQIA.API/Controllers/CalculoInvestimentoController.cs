using CalculadoraSQIA.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalculadoraSQIA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculoInvestimentoController : ControllerBase
    {
        private readonly ICalculoService _calculoService;
        private readonly ILogger<CalculoInvestimentoController> _logger;

        public CalculoInvestimentoController(ICalculoService calculoService, ILogger<CalculoInvestimentoController> logger)
        {
            _calculoService = calculoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Calcular([FromQuery] decimal valor,
                                                  [FromQuery] DateTime dataInicio,
                                                  [FromQuery] DateTime dataFim)
        {
            _logger.LogInformation("Iníco da chamado Calcular da Controller");

            if (dataInicio >= dataFim)
            {
                _logger.LogWarning("Data inicial deve ser menor que data final.");

                return BadRequest("Data inicial deve ser menor que data final.");
            }

            var resultado = await _calculoService.Executar(valor, dataInicio, dataFim);

            _logger.LogInformation("Fim da chamada Calcular da Controller");
            return Ok(resultado);
        }
    }
}
