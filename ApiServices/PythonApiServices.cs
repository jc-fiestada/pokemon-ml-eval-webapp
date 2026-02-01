using PokemonMlEvalWebApp.Models;

namespace PokemonMlEvalWebApp.ApiServices;

public class PythonApiService
{
    private readonly HttpClient _httpCLient;

    public PythonApiService(HttpClient client)
    {
        _httpCLient = client;
        _httpCLient.BaseAddress = new Uri("http://127.0.0.1:8000/");
        _httpCLient.Timeout = TimeSpan.FromSeconds(10);
    }

    public async Task<PokemonEvalDTO> TrainAndTestModels(int quantity, int randomState)
    {
        var response = await _httpCLient.PostAsJsonAsync("predict/pokemon-type", new UserModelRequest(quantity, randomState));

        response.EnsureSuccessStatusCode(); // make sure to catch this outside

        return await response.Content.ReadFromJsonAsync<PokemonEvalDTO>() ?? throw new InvalidOperationException("Deserialization returned a null value");
    }
}