using ApiInteligenteTareas.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiInteligenteTareas.Controllers;

[Route("api/tareas-externas")]
[ApiController]
public class TareasExternasController : ControllerBase
{
    private readonly TareasExternasService _service;

    public TareasExternasController(TareasExternasService service)
    {
        _service = service;
    }

    // GET /api/tareas-externas
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var tareas = await _service.ObtenerTodasAsync();
            return Ok(tareas);
        }
        catch (HttpRequestException)
        {
            return StatusCode(503, "No se pudo conectar con la API externa. Intente más tarde.");
        }
    }

    // GET /api/tareas-externas/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var tarea = await _service.ObtenerPorIdAsync(id);
            if (tarea is null)
                return NotFound($"No se encontró una tarea externa con id {id}.");

            return Ok(tarea);
        }
        catch (HttpRequestException)
        {
            return StatusCode(503, "No se pudo conectar con la API externa. Intente más tarde.");
        }
    }
}
