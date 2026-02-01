using System.Text.Json.Serialization;

namespace PokemonMlEvalWebApp.Models;

public class PokemonEvalDTO
{
    [JsonPropertyName("model_eval")]
    required public List<PokemonEval> ModelEval {get; set;}
    [JsonPropertyName("knn_metrics")]
    required public Metrics KnnMetrics {get; set;}
    [JsonPropertyName("log_reg_metrics")]
    required public Metrics LogRegMetrics {get; set;}
    [JsonPropertyName("tree_metrics")]
    required public Metrics TreeMetrics {get; set;}
}