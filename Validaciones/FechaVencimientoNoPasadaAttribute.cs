using System.ComponentModel.DataAnnotations;

namespace ApiInteligenteTareas.Validaciones;

public class FechaVencimientoNoPasadaAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime fecha && fecha.Date < DateTime.Today)
            return new ValidationResult("La fecha de vencimiento no puede ser anterior a la fecha actual.");

        return ValidationResult.Success;
    }
}
