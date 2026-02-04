using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;
using Microsoft.IdentityModel.Tokens.Experimental;
using PokemonMlEvalWebApp.Models;
using PokemonMlEvalWebApp.Validators;
using PokemonMlEvalWebApp.MysqlService;
using PokemonMlEvalWebApp.ApiServices;
using System.Xml.Serialization;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<Validate>();
builder.Services.AddScoped<Service>();
builder.Services.AddScoped<PokemonApiServices>();
builder.Services.AddHttpClient<PythonApiService>();
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
    if (await service.IsDataExists()) return Results.Conflict("Pokemon Data Already Exist's");

    try
    {
        await apiService.ManagePokemonDataSet(service);
    } catch (Exception ex)
    {
        Console.WriteLine(ex);
        return Results.InternalServerError("ServerError: Something went wrong");
    }
    return Results.Ok("success");
}).RequireAuthorization();

app.MapPost("/predict/pokemon", async (UserModelRequest request, PythonApiService service) =>
{
    if (request.RandomState < 1) return Results.UnprocessableEntity("Invalid RandomState Value");

    if (request.Quantity < 7 || request.Quantity > 150) 
    return Results.UnprocessableEntity("Invalid Quantity - Must be above 7 or below 150");

    PokemonEvalDTO response;

    try
    {
        response = await service.TrainAndTestModels(request.Quantity, request.RandomState);
    } catch (InvalidOperationException ex)
    {
        return Results.InternalServerError(ex);
    } catch (Exception ex) {
        Console.WriteLine(ex);
        return Results.InternalServerError("ServerError: Something went wrong");
    }

    return Results.Ok(response);
}).RequireAuthorization();


app.Run();