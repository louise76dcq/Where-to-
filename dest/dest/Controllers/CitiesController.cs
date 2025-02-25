using Microsoft.AspNetCore.Mvc;
using TravelPlannerAPI.Data;
using TravelPlannerAPI.Models;


[Route("api/[controller]")]
[ApiController]
public class CitiesController : ControllerBase
{
    private readonly TravelDbContext _context;

    public CitiesController(TravelDbContext  context)
    {
        _context = context;
    }

    // POST: api/Cities
    [HttpPost]
    public async Task<ActionResult<Destination>> PostCity(CityDto cityDto)
    {
        var city = new Destination
        {
            name = cityDto.Name,
            image_url = cityDto.ImageUrl,
            description = cityDto.Description,
            iatacode = cityDto.IataCode // Ajout du IataCode
        };

        _context.destinations.Add(city);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCity), new { id = city.id }, city);
    }

    // Optionnel, pour récupérer une ville par son ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Destination>> GetCity(int id)
    {
        var city = await _context.destinations.FindAsync(id);

        if (city == null)
        {
            return NotFound();
        }

        return city;
    }
}