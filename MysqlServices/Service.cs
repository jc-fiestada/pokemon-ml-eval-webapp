using MySqlConnector;
using PokemonMlEvalWebApp.Models;
using BCrypt.Net;

namespace PokemonMlEvalWebApp.MysqlService;

public class Service
{
    // db already pre created manually, just select only no modification
    string dbConn;

    //
    public Service()
    {
        string dbPassword = Environment.GetEnvironmentVariable("MSQL_PASSWORD") ?? throw new InvalidOperationException("ERROR: database key missing");
        dbConn = "Server=localhost;" + "Database=pokemon_admin; " +  "User=root;" + $"Password={dbPassword};";
    }

    // one time use only - for admin credential insertion with hash password 
    /*
    public async Task InsertAdminCredentials(Admin admin)
    {
        string hashedPass = BCrypt.Net.BCrypt.HashPassword(admin.Password);

        using MySqlConnection conn = new MySqlConnection(dbConn);
        await conn.OpenAsync();

        string query = "INSERT INTO admin values (@username, @password);";
        using var command = new MySqlCommand(query, conn);
        command.Parameters.AddWithValue("@username", admin.Username);
        command.Parameters.AddWithValue("@password", hashedPass);

        await command.ExecuteNonQueryAsync();
    }
    */

    public async Task<Admin> SelectAdmin()
    {
        using MySqlConnection conn = new MySqlConnection(dbConn);
        await conn.OpenAsync();

        string query = "SELECT * FROM admin";
        using MySqlCommand command = new MySqlCommand(query, conn);
        using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync()) throw new InvalidOperationException("ERROR: admin credentials failed to load");

        return new Admin()
        {
            Username = reader.GetString("username"),
            Password = reader.GetString("password"),
            AdminUsername = reader.GetString("admin_username")
        };
    }

    public async Task CreatePokemonTable()
    {
        using MySqlConnection conn = new MySqlConnection(dbConn);
        await conn.OpenAsync();

        string query = @"CREATE TABLE IF NOT EXISTS pokemon (
            id INT PRIMARY KEY AUTO_INCREMENT,
            name VARCHAR(255),
            type VARCHAR(255),
            health INT,
            defense INT,
            weight INT,
            attack INT,
            speed INT,
            height INT,
            special_attack INT,
            special_defense INT);";

        using MySqlCommand command = new MySqlCommand(query, conn);
        await command.ExecuteNonQueryAsync();
    }

    public async Task InsertPokemon(Pokemon pokemon)
    {
        await CreatePokemonTable();

        using MySqlConnection conn = new MySqlConnection(dbConn);
        await conn.OpenAsync();

        string query = @"INSERT INTO pokemon (name, type, health, defense, weight, attack, speed, height, special_attack, special_defense) 
        VALUES (@name, @type, @health, @defense, @weight, @attack, @speed, @height, @special_attack, @special_defense)";

        MySqlCommand command = new MySqlCommand(query, conn);
        command.Parameters.AddWithValue("@name", pokemon.Name);
        command.Parameters.AddWithValue("@type", pokemon.Primarytype);
        command.Parameters.AddWithValue("@health", pokemon.Health);
        command.Parameters.AddWithValue("@defense", pokemon.Defense);
        command.Parameters.AddWithValue("@weight", pokemon.Weight);
        command.Parameters.AddWithValue("@attack", pokemon.Attack);
        command.Parameters.AddWithValue("@speed", pokemon.Speed);
        command.Parameters.AddWithValue("@height", pokemon.Height);
        command.Parameters.AddWithValue("@special_attack", pokemon.SpecialAttack);
        command.Parameters.AddWithValue("@special_defense", pokemon.SpecialDefense);

        await command.ExecuteNonQueryAsync();
    }



    public async Task<bool> IsDataExists()
    {
        await CreatePokemonTable();

        using MySqlConnection conn = new MySqlConnection(dbConn);
        await conn.OpenAsync();

        string query = "SELECT * FROM pokemon LIMIT 1";

        using MySqlCommand command = new MySqlCommand(query, conn);
        using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync()) return false;
        return true;
    }
}