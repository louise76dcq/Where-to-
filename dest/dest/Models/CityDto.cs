namespace TravelPlannerAPI.Models;

public class CityDto
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string Description { get; set; }
    public string IataCode { get; set; } // Nouveau champ IataCode
}