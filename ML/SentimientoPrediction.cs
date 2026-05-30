using Microsoft.ML.Data;

namespace ApiInteligenteTareas.ML;

public class SentimientoPrediction
{
    [ColumnName("PredictedLabel")]
    public bool PredictedLabel { get; set; }

    public float Probability { get; set; }

    public float Score { get; set; }
}
