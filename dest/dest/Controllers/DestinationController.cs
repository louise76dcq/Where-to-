using Microsoft.AspNetCore.Mvc;
using dest.Services;
using dest.Models;

namespace dest.Controllers
{
    [ApiController]
    [Route("api/destination")]
    public class DestinationController : ControllerBase
    {
        private readonly DestinationService _destinationService;

        public DestinationController(DestinationService destinationService)
        {
            _destinationService = destinationService;
        }

        [HttpGet("random")]
        public IActionResult GetRandom()
        {
            try
            {
                var destination = _destinationService.GetRandomDestination();
                return Ok(destination);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("add")]
        public IActionResult AddDestination([FromBody] Destination destination)
        {
            try
            {
                _destinationService.AddDestination(destination);
                return Ok("Destination ajoutée avec succès !");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("random/filtered")]
        public IActionResult GetFilteredRandom([FromQuery] string environment, [FromQuery] decimal maxPrice)
        {
            try
            {
                var destination = _destinationService.GetFilteredRandomDestination(environment, maxPrice);
                return Ok(destination);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}