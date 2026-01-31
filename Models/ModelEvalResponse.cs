using System.Text.Json.Serialization;

namespace PokemonMlEvalWebApp.Models;

public class PokemonResponse
{
    [JsonPropertyName("Id")]
    public int Id {get; set;}
    [JsonPropertyName("name")]
    required public string Name {get; set;}
    [JsonPropertyName("type")]
    required public string Primarytype {get; set;}
    [JsonPropertyName("health")]
    public int Health {get; set;}
    [JsonPropertyName("defense")]
    public int Defense {get; set;}
    [JsonPropertyName("weight")]
    public int Weight {get; set;}
    [JsonPropertyName("attack")]
    public int Attack {get; set;}
    [JsonPropertyName("speed")]
    public int Speed {get; set;}
    [JsonPropertyName("height")]
    public int Height {get; set;}
    [JsonPropertyName("special_attack")]
    public int SpecialAttack {get; set;}
    [JsonPropertyName("special_defense")]
    public int SpecialDefense {get; set;}
}

public class ModelMetrics
{
    [JsonPropertyName("f1_score")]
    public float F1Score {get; set;}
    [JsonPropertyName("accuracy_score")]
    public float Accuracy {get; set;}
    [JsonPropertyName("recall_score")]
    public float Recall {get; set;}
    [JsonPropertyName("precision_score")]
    public float Precision {get; set;}
}

public class ModelEvalResponse
{
    [JsonPropertyName("KNN")]
    required public List<PokemonResponse> Knn {get; set;}
    [JsonPropertyName("KNN_Metrics")]
    required public ModelMetrics KnnMetrics {get; set;}
    [JsonPropertyName("LogReg")]
    required public List<PokemonResponse> LogReg {get; set;}
    [JsonPropertyName("LogReg_Metrics")]
    required public ModelMetrics LogRegMetrics {get; set;}
    [JsonPropertyName("Tree")]
    required public List<PokemonResponse> Tree {get; set;}
    [JsonPropertyName("Tree_Metrics")]
    required public ModelMetrics TreeMetrics {get; set;}
}