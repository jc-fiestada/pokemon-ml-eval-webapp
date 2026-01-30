using System.Text.Json;
using Microsoft.VisualBasic;
using PokemonMlEvalWebApp.Models;
using PokemonMlEvalWebApp.MysqlService;

namespace PokemonMlEvalWebApp.ApiServices;

public class PokemonApiServices
{
    private readonly HttpClient _httpClient;
    string unproccessedDir = Path.Combine("Data", "Unprocessed");

    public PokemonApiServices(HttpClient client)
    {
        _httpClient = client;
        _httpClient.BaseAddress = new Uri("https://pokeapi.co/api/v2/pokemon/");
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    public void Show()
    {
        Console.WriteLine(unproccessedDir);
    }

    // dont overuse? ts is heavy
    public async Task ManagePokemonDataSet(Service service)
    {
        if (!Directory.Exists(unproccessedDir)) Directory.CreateDirectory(unproccessedDir);
        IEnumerable<string> files = Directory.EnumerateFiles(unproccessedDir);
        if (files.Count() >= 150) return;

        await LoadRawJsonFiles();
        await ProcessRawPokemon(service);
        DeleteRawJsonFiles();
    }

    private async Task LoadRawJsonFiles(int quantity = 150)
    {
        if (quantity > 150) quantity = 150;

        for (int i = 1; i <= quantity; i++)
        {
            try
            {
                string filepath = Path.Combine(unproccessedDir, $"{i}.json");
                if (File.Exists(filepath)) continue;

                HttpResponseMessage response = await _httpClient.GetAsync($"{i}");
                if (!response.IsSuccessStatusCode) continue;

                string jsonContent = await response.Content.ReadAsStringAsync();
                File.WriteAllText(filepath, jsonContent);
            } catch (Exception)
            {
                continue;
            }
        }
    }

    private void DeleteRawJsonFiles()
    {
        if (!Directory.Exists(unproccessedDir)) return;
        foreach (var file in Directory.EnumerateFiles(unproccessedDir))
        {
            File.Delete(file);
        }
    }

    private async Task ProcessRawPokemon(Service service)
    {
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
            await service.InsertPokemon(pokemon);
        }
    }
}

