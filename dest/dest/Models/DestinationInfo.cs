using System.ComponentModel.DataAnnotations;

namespace TravelPlannerAPI.Models
{
    public class DestinationInfo
    {
        [Key]
        public int id { get; set; }
        public string iatacode { get; set; } // Code IATA (ex: LIS, BCN)
        public string name { get; set; } // Nom de la destination
        public string description { get; set; } // Description touristique
        public string imageUrl { get; set; } // URL de l'image
    }
}