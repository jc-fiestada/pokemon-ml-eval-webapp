using Microsoft.IdentityModel.Tokens;
using PokemonMlEvalWebApp.Models;
using PokemonMlEvalWebApp.MysqlService;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PokemonMlEvalWebApp.Validators;

public class Validate
{
    public async Task<IResult> ValidateSignInRequest(SignInRequest data, Service service, byte[] keyBytes)
    {
        if (string.IsNullOrWhiteSpace(data.Username)) return Results.BadRequest("Username is required");
        if (string.IsNullOrWhiteSpace(data.Password)) return Results.BadRequest("Password is required");

        if (data.Username.Length > 15) return Results.BadRequest("Username must not exceed 15 characters");
        if (data.Password.Length > 15) return Results.BadRequest("Password must not exceed 15 characters");

        Admin admin = await service.SelectAdmin();

        if (data.Username != admin.Username || !BCrypt.Net.BCrypt.Verify(data.Password, admin.Password)) return Results.Unauthorized();

        Claim[] userClaims = new Claim[]
        {
            new Claim(ClaimTypes.Name, admin.AdminUsername),
            new Claim(ClaimTypes.Role, "Role")
        };

        JwtSecurityToken token = new JwtSecurityToken(
            claims: userClaims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256)
        );

        string jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Results.Json(new { token = jwt}, statusCode: 200);
    }
}