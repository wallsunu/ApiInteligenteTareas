using ApiInteligenteTareas.ML;
using Microsoft.ML;

namespace ApiInteligenteTareas.Services;

public class SentimientoService
{
    private readonly PredictionEngine<SentimientoData, SentimientoPrediction> _predictionEngine;

    public SentimientoService(IWebHostEnvironment env)
    {
        var mlContext = new MLContext(seed: 0);

        var dataPath = Path.Combine(env.ContentRootPath, "ML", "sentimiento-dataset.csv");

        var data = mlContext.Data.LoadFromTextFile<SentimientoData>(
            path: dataPath,
            hasHeader: true,
            separatorChar: ',');

        var pipeline = mlContext.Transforms.Text
            .FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(SentimientoData.Comentario))
            .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                labelColumnName: "Label",
                featureColumnName: "Features"));

        var model = pipeline.Fit(data);

        _predictionEngine = mlContext.Model.CreatePredictionEngine<SentimientoData, SentimientoPrediction>(model);
    }

    public string Analizar(string comentario)
    {
        var input = new SentimientoData { Comentario = comentario };
        var prediction = _predictionEngine.Predict(input);
        return prediction.PredictedLabel ? "Positivo" : "Negativo";
    }
}
