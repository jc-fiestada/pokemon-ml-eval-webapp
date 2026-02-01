using System.Text.Json.Serialization;

namespace PokemonMlEvalWebApp.Models;

public class PokemonEval
{
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
    [JsonPropertyName("id")]
    public int Id {get; set;}
    [JsonPropertyName("name")]
    public string Name {get; set;} = string.Empty;
    [JsonPropertyName("true_value")]
    public string TrueType {get; set;} = string.Empty;
    [JsonPropertyName("knn_prediction")]
    public string KNN {get; set;} = string.Empty;
    [JsonPropertyName("log_reg_prediction")]
    public string LogReg {get; set;} = string.Empty;
    [JsonPropertyName("tree_prediction")]
    public string Tree {get; set;} = string.Empty;
}