namespace PokemonMlEvalWebApp.ApiServices;

public class UvicornApiService
{
    private readonly HttpClient _httpCLient;

    public UvicornApiService(HttpClient client)
    {
        _httpCLient = client;
        _httpCLient.BaseAddress = new Uri(""); // add later
        _httpCLient.Timeout = TimeSpan.FromSeconds(10);
    }

    
}