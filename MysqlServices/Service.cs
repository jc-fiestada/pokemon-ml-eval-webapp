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
        string dbPassword = Environment.GetEnvironmentVariable("MSQL_PASSWORD") ?? throw new Exception("ERROR: database key missing");
        dbConn = "Server=localhost;" + "Database=pokemon_admin; " +  "User=root;" + $"Password={dbPassword};";
    }

    // one time use only - for admin credential insertion with hash password 
    /*
    public async Task InsertAdminCredentials(Admin admin)
    {
        string hashedPass = BCrypt.Net.BCrypt.HashPassword(admin.Password);

        using var conn = new MySqlConnection(dbConn);
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
        using var conn = new MySqlConnection(dbConn);
        await conn.OpenAsync();

        string query = "SELECT * FROM admin";
        using var command = new MySqlCommand(query, conn);
        using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync()) throw new Exception("ERROR: admin credentials failed to load");

        return new Admin()
        {
            Username = reader.GetString("username"),
            Password = reader.GetString("password"),
            AdminUsername = reader.GetString("admin_username")
        };
    }
}