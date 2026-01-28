using PokemonMlEvalWebApp.Models;
namespace PokemonMlEvalWebApp.Validators;

public class Validate
{
    public IResult ValidateSignInRequest(SignInRequest data)
    {
        if (string.IsNullOrWhiteSpace(data.Username)) return Results.BadRequest("Username is required");
        if (string.IsNullOrWhiteSpace(data.Password)) return Results.BadRequest("Password is required");

        if (data.Username.Length > 15) return Results.BadRequest("Username must not exceed 15 characters");
        if (data.Password.Length > 15) return Results.BadRequest("Password must not exceed 15 characters");

        return Results.Ok();
    }
}