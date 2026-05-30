using ApiInteligenteTareas.Data;
using ApiInteligenteTareas.DTOs;
using ApiInteligenteTareas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiInteligenteTareas.Controllers;

[Route("api/tareas")]
[ApiController]
public class TareasController : ControllerBase
{
    private readonly AppDbContext _context;

    public TareasController(AppDbContext context)
    {
        _context = context;
    }

    // GET /api/tareas
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tareas = await _context.Tareas.OrderBy(t => t.Id).ToListAsync();
        return Ok(tareas);
    }

    // GET /api/tareas/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);
        if (tarea is null)
            return NotFound();

        return Ok(tarea);
    }

    // POST /api/tareas
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TareaCreateDto dto)
    {
        if (!Enum.IsDefined(typeof(EstadoTarea), dto.Estado!))
            return BadRequest("El valor de Estado no es válido.");

        if (!Enum.IsDefined(typeof(PrioridadTarea), dto.Prioridad!))
            return BadRequest("El valor de Prioridad no es válido.");

        var tarea = new Tarea
        {
            Titulo = dto.Titulo,
            Descripcion = dto.Descripcion,
            Estado = dto.Estado!.Value,
            Prioridad = dto.Prioridad!.Value,
            FechaCreacion = DateTime.Now,
            FechaVencimiento = dto.FechaVencimiento
        };

        _context.Tareas.Add(tarea);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = tarea.Id }, tarea);
    }

    // PUT /api/tareas/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TareaUpdateDto dto)
    {
        var tarea = await _context.Tareas.FindAsync(id);
        if (tarea is null)
            return NotFound();

        if (!Enum.IsDefined(typeof(EstadoTarea), dto.Estado!))
            return BadRequest("El valor de Estado no es válido.");

        if (!Enum.IsDefined(typeof(PrioridadTarea), dto.Prioridad!))
            return BadRequest("El valor de Prioridad no es válido.");

        tarea.Titulo = dto.Titulo;
        tarea.Descripcion = dto.Descripcion;
        tarea.Estado = dto.Estado!.Value;
        tarea.Prioridad = dto.Prioridad!.Value;
        tarea.FechaVencimiento = dto.FechaVencimiento;

        await _context.SaveChangesAsync();

        return Ok(tarea);
    }

    // DELETE /api/tareas/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);
        if (tarea is null)
            return NotFound();

        _context.Tareas.Remove(tarea);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
