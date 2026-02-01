using System.Text.Json.Serialization;

namespace PokemonMlEvalWebApp.Models;

public class Metrics
{
    [JsonPropertyName("f1_score")]
    public float F1Score {get; set;}
    [JsonPropertyName("accuracy_score")]
    public float AccuracyScore {get; set;}
    [JsonPropertyName("precision_score")]
    public float PrecisionScore {get; set;}
    [JsonPropertyName("recall_score")]
    public float RecallScore {get; set;}
}