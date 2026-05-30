using Microsoft.ML.Data;

namespace ApiInteligenteTareas.ML;

public class SentimientoData
{
    [LoadColumn(0)]
    public string Comentario { get; set; } = string.Empty;

    [LoadColumn(1), ColumnName("Label")]
    public bool Sentimiento { get; set; }
}
