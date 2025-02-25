using Npgsql;
using dest.Models;

namespace dest.Services
{
    public class DestinationService
    {
        private readonly SqlConnection _sqlConnection;

        public DestinationService(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public Destination GetRandomDestination()
        {
            using var conn = _sqlConnection.GetConnection();
            var cmd = new NpgsqlCommand("SELECT name, price, photo_url, description, environment FROM destinations ORDER BY RANDOM() LIMIT 1;", conn);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Destination
                {
                    Name = reader.GetString(0),
                    Price = reader.GetDecimal(1),
                    PhotoUrl = reader.GetString(2),
                    Description = reader.GetString(3),
                    Environment = reader.GetString(4)
                };
            }

            throw new Exception("Aucune destination trouvée.");
        }

        public void AddDestination(Destination destination)
        {
            using var conn = _sqlConnection.GetConnection();
            var cmd = new NpgsqlCommand(
                "INSERT INTO destinations (name, price, photo_url, description, environment) VALUES (@name, @price, @photo_url, @description, @environment);",
                conn
            );

            cmd.Parameters.AddWithValue("name", destination.Name);
            cmd.Parameters.AddWithValue("price", destination.Price);
            cmd.Parameters.AddWithValue("photo_url", destination.PhotoUrl);
            cmd.Parameters.AddWithValue("description", destination.Description);
            cmd.Parameters.AddWithValue("environment", destination.Environment);

            cmd.ExecuteNonQuery();
        }
        
        public Destination GetFilteredRandomDestination(string environment, decimal maxPrice)
        {
            using var conn = _sqlConnection.GetConnection();
            var cmd = new NpgsqlCommand("SELECT name, price, photo_url, description, environment FROM destinations WHERE environment = @environment AND price <= @maxPrice ORDER BY RANDOM() LIMIT 1;", conn);
            
            cmd.Parameters.AddWithValue("environment", environment);
            cmd.Parameters.AddWithValue("maxPrice", maxPrice);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new Destination
                {
                    Name = reader.GetString(0),
                    Price = reader.GetDecimal(1),
                    PhotoUrl = reader.GetString(2),
                    Description = reader.GetString(3),
                    Environment = reader.GetString(4)
                };
            }

            throw new Exception("Aucune destination trouvée avec ces critères.");
        }
    }
}