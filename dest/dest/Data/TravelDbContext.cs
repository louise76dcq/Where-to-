using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Data
{
    public class TravelDbContext : DbContext
    {
        public TravelDbContext(DbContextOptions<TravelDbContext> options)
            : base(options)
        { }

        // Assure-toi que la propriété DbSet<City> existe
        public DbSet<Destination> destinations { get; set; }

        // Tu peux aussi ajouter d'autres DbSets pour d'autres entités si nécessaire
    }
}