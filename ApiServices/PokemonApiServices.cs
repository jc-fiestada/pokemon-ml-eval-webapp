using System.Text.Json;
using PokemonMlEvalWebApp.Models;

namespace PokemonMlEvalWebApp.ApiServices;

public class PokemonApiServices
{
    private readonly HttpClient _httpClient;
    string unproccessedDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "Data", "Unprocessed"));

    public PokemonApiServices(HttpClient client)
    {
        _httpClient = client;
        _httpClient.BaseAddress = new Uri("https://pokeapi.co/api/v2/pokemon/");
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    // use only to populate db then delete later
    public async Task LoadRawJsonFiles(int quantity = 150)
    {
        if (quantity > 150) quantity = 150;
        if (!Directory.Exists(unproccessedDir)) Directory.CreateDirectory(unproccessedDir);

        if (Directory.EnumerateFiles(unproccessedDir).Count() >= 150) return;

        for (int i = 0; i < quantity; i++)
        {
            try
            {
                string filepath = Path.Combine(unproccessedDir, $"{i}.json");
                if (File.Exists(filepath)) continue;

                var response = await _httpClient.GetAsync($"{i}");
                if (!response.IsSuccessStatusCode) continue;

                var jsonContent = await response.Content.ReadAsStringAsync();
                File.WriteAllText(filepath, jsonContent);
            } catch (Exception)
            {
                continue;
            }
        }
        
    }

    public void DeleteRawJsonFiles()
    {
        if (!Directory.Exists(unproccessedDir)) return;
        foreach (var file in Directory.EnumerateFiles(unproccessedDir))
        {
            File.Delete(file);
        }
    }

    public async Task ProcessRawPokemon()
    {
        if (!Directory.Exists(unproccessedDir) || Directory.EnumerateFiles(unproccessedDir).Count() == 0 )
        throw new Exception("ERROR: Json files does not exist's");

        foreach (var file in Directory.EnumerateFiles(unproccessedDir))
        {
            string jsonContent = await File.ReadAllTextAsync(file);

            using var doc = JsonDocument.Parse(jsonContent);
            var root = doc.RootElement;

            Pokemon pokemon = new Pokemon();
            pokemon.Name = root.GetProperty("name").GetString() ?? "Unknown";
            pokemon.Primarytype = root.GetProperty("types")[0].GetProperty("type").GetProperty("name").GetString();
            pokemon.Height = root.GetProperty("height").GetInt32();
            pokemon.Weight = root.GetProperty("weight").GetInt32();

            foreach (var stat in root.GetProperty("stats").EnumerateArray())
            {
                switch (stat.GetProperty("stat").GetProperty("name").GetString())
                {
                    case "hp":
                        pokemon.Health = stat.GetProperty("base_stat").GetInt32();
                    break;
                    case "attack":
                        pokemon.Attack = stat.GetProperty("base_stat").GetInt32();
                    break;
                    case "defense":
                        pokemon.Defense = stat.GetProperty("base_stat").GetInt32();
                    break;  
                    case "special-attack":
                        pokemon.SpecialAttack = stat.GetProperty("base_stat").GetInt32();
                    break;
                    case "special-defense":
                        pokemon.SpecialDefense = stat.GetProperty("base_stat").GetInt32();
                    break;
                    case "speed":
                        pokemon.Speed = stat.GetProperty("base_stat").GetInt32();
                    break;
                }
            }

            // db functionality here, ill add it later 

        }


    }
}

