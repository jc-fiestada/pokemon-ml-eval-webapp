using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;
using Microsoft.IdentityModel.Tokens.Experimental;
using PokemonMlEvalWebApp.Models;
using PokemonMlEvalWebApp.Validators;
using PokemonMlEvalWebApp.MysqlService;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<Validate>();

Env.Load("keys.env");
string jwtSecretKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new Exception("ERROR: jwt secret key is missing");
byte[] keybytes = Encoding.UTF8.GetBytes(jwtSecretKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keybytes)
        };
    }
);

builder.Services.AddAuthorization();

var app = builder.Build();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("validate/signin", (SignInRequest user, Validate service) =>
{
    
});

app.MapGet("/try", async () =>
{
    
    Service service =  new Service();

    return Results.Ok(new {admin = await service.SelectAdmin()});
});

app.Run();
