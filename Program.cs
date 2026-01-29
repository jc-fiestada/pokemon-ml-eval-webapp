using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;
using Microsoft.IdentityModel.Tokens.Experimental;
using PokemonMlEvalWebApp.Models;
using PokemonMlEvalWebApp.Validators;
using PokemonMlEvalWebApp.MysqlService;
using PokemonMlEvalWebApp.ApiServices;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<Validate>();
builder.Services.AddScoped<Service>();
builder.Services.AddScoped<PokemonApiServices>();
builder.Services.AddHttpClient<PokemonApiServices>();

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


app.MapPost("/signin/admin", async (SignInRequest user, Service service, Validate validate) =>
{
    return await validate.ValidateSignInRequest(user, service, keybytes);
});

app.MapGet("/verify/page-access", () =>
{
    return Results.Ok("User Authenticated");
}).RequireAuthorization();

app.MapGet("/store/db/pokemon", async (PokemonApiServices apiService, Service service) =>
{
    await apiService.ManagePokemonDataSet(service);
    return Results.Ok("success");
}).RequireAuthorization();

app.MapGet("/check", (PokemonApiServices apiService) =>
{
    apiService.Show();
});

app.Run();
