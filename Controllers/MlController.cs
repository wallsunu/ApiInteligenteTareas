using ApiInteligenteTareas.DTOs;
using ApiInteligenteTareas.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiInteligenteTareas.Controllers;

[Route("api/ml")]
[ApiController]
public class MlController : ControllerBase
{
    private readonly SentimientoService _sentimientoService;

    public MlController(SentimientoService sentimientoService)
    {
        _sentimientoService = sentimientoService;
    }

    // POST /api/ml/sentimiento
    [HttpPost("sentimiento")]
    public IActionResult AnalizarSentimiento([FromBody] SentimientoRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Comentario))
            return BadRequest("El comentario no puede ser nulo, vacío o contener solo espacios.");

        var sentimiento = _sentimientoService.Analizar(request.Comentario);

        return Ok(new SentimientoResponseDto
        {
            Comentario = request.Comentario,
            Sentimiento = sentimiento
        });
    }
}
