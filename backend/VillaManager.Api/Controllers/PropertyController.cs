using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VillaManager.Infrastructure.Data;

namespace VillaManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyController : ControllerBase
{
    private readonly VillaManagerDbContext _dbContext;
    private readonly ILogger<PropertyController> _logger;

    public PropertyController(VillaManagerDbContext dbContext, ILogger<PropertyController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Get a specific property by ID
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<object>> GetProperty(int id)
    {
        try
        {
            var property = await _dbContext.Properties
                .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null)
                return NotFound(new { message = "Property not found" });

            return Ok(new
            {
                id = property.Id,
                name = property.Name,
                description = property.Description,
                location = property.Location,
                pricePerNight = property.PricePerNight,
                maxGuests = property.MaxGuests,
                imageUrls = property.ImageUrls
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting property {PropertyId}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get all properties
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<object>>> GetProperties()
    {
        try
        {
            var properties = await _dbContext.Properties
                .Select(p => new
                {
                    id = p.Id,
                    name = p.Name,
                    description = p.Description,
                    location = p.Location,
                    pricePerNight = p.PricePerNight,
                    maxGuests = p.MaxGuests,
                    imageUrls = p.ImageUrls
                })
                .ToListAsync();

            return Ok(properties);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting properties");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}
