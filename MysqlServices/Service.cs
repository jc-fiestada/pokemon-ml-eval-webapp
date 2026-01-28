using MySqlConnector;
using PokemonMlEvalWebApp.Models;

namespace PokemonMlEvalWebApp.MysqlServices;

public class Service
{
    string dbConn;
    public Service()
    {
        string dbPassword = Environment.GetEnvironmentVariable("MSQL_PASSWORD") ?? throw new Exception("ERROR: database key missing");
        dbConn = "Server=localhost;" + "Database=pokemon_admin" +  "User=root;" + $"Password={dbPassword}";
    }

    public async Task<Admin> SelectAdmin()
    {
        using var conn = new MySqlConnection(dbConn);
        await conn.OpenAsync();

        string query = "SELECT * FROM admin";

        using var command = new MySqlCommand(query, conn);
        using var reader = command.ExecuteReader();

        if (!await reader.ReadAsync()) throw new Exception("ERROR: admin credentials failed to load");

        return new Admin()
        {
            Username = reader.GetString("username"),
            Password = reader.GetString("password")
        };
    }
}