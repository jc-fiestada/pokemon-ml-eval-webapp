using System.Text.Json.Serialization;

namespace PokemonMlEvalWebApp.Models;

public class PokemonEvalDTO
{
    [JsonPropertyName("model_eval")]
    public List<PokemonEval> ModelEval {get; set;}
}