using Npgsql;
using Microsoft.Extensions.Configuration;

namespace dest.Services
{
    public class SqlConnection
    {
        private readonly IConfiguration _configuration;

        public SqlConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public NpgsqlConnection GetConnection()
        {
            var connectionString = _configuration.GetConnectionString("PostgreSQL");
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}