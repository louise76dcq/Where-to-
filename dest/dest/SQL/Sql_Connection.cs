using Npgsql;
using DotNetEnv;

namespace dest.SQL;

public class SqlConnection
{
    public static NpgsqlConnection GetConnection()
    {
        var envPath = Path.Combine(Directory.GetCurrentDirectory(), "API", "SQL", "Database.env");
        Env.Load(envPath);
        
        var host = Env.GetString("DB_HOST");
        var port = Env.GetString("DB_PORT");
        var database = Env.GetString("DB_DB");
        var username = Env.GetString("DB_USER");
        var password = Env.GetString("DB_PASSWORD");
        
        var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};";
        
        Console.WriteLine(connectionString);

        var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        return connection;
    }
}