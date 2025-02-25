namespace TravelPlannerAPI.Models;

public class Destination
{
    public int id { get; set; }
    public string name { get; set; }
    public string image_url { get; set; }
    public string description { get; set; } // Assure-toi que cette propriété est bien présente
    public string iatacode { get; set; }
}
