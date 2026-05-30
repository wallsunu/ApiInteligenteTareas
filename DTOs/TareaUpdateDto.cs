using System.ComponentModel.DataAnnotations;
using ApiInteligenteTareas.Models;
using ApiInteligenteTareas.Validaciones;

namespace ApiInteligenteTareas.DTOs;

public class TareaUpdateDto
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    public string Titulo { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El estado es obligatorio.")]
    public EstadoTarea? Estado { get; set; }

    [Required(ErrorMessage = "La prioridad es obligatoria.")]
    public PrioridadTarea? Prioridad { get; set; }

    [FechaVencimientoNoPasada]
    public DateTime FechaVencimiento { get; set; }
}
