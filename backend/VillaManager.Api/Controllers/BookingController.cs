using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VillaManager.Core.Entities;
using VillaManager.Core.Enums;
using VillaManager.Infrastructure.Data;

namespace VillaManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly VillaManagerDbContext _dbContext;
    private readonly ILogger<BookingController> _logger;

    public BookingController(VillaManagerDbContext dbContext, ILogger<BookingController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Get all bookings (Admin only)
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<object>>> GetBookings()
    {
        try
        {
            var bookings = await _dbContext.Bookings
                .Select(b => new
                {
                    id = b.Id,
                    propertyId = b.PropertyId,
                    guestName = b.GuestName,
                    guestEmail = b.GuestEmail,
                    guestPhone = b.GuestPhone,
                    checkInDate = b.CheckInDate,
                    checkOutDate = b.CheckOutDate,
                    numberOfGuests = b.NumberOfGuests,
                    totalPrice = b.TotalPrice,
                    status = b.Status.ToString(),
                    externalPlatform = b.ExternalPlatform,
                    createdAt = b.CreatedAt
                })
                .ToListAsync();

            return Ok(bookings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bookings");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get a specific booking by ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<object>> GetBooking(int id)
    {
        try
        {
            var booking = await _dbContext.Bookings
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound(new { message = "Booking not found" });

            return Ok(new
            {
                id = booking.Id,
                propertyId = booking.PropertyId,
                guestName = booking.GuestName,
                guestEmail = booking.GuestEmail,
                guestPhone = booking.GuestPhone,
                checkInDate = booking.CheckInDate,
                checkOutDate = booking.CheckOutDate,
                numberOfGuests = booking.NumberOfGuests,
                totalPrice = booking.TotalPrice,
                status = booking.Status.ToString(),
                externalPlatform = booking.ExternalPlatform,
                createdAt = booking.CreatedAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting booking {BookingId}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Create a new booking inquiry
    /// </summary>
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<object>> CreateBooking([FromBody] CreateBookingRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var booking = new Booking
            {
                PropertyId = request.PropertyId,
                GuestName = request.GuestName,
                GuestEmail = request.GuestEmail,
                GuestPhone = request.GuestPhone,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                NumberOfGuests = request.NumberOfGuests,
                TotalPrice = request.TotalPrice,
                Status = BookingStatus.Pending,
                ExternalPlatform = "direct",
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Bookings.Add(booking);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("New booking created with ID {BookingId}", booking.Id);

            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, new
            {
                id = booking.Id,
                message = "Booking inquiry received. We will contact you soon!"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Update booking status (Admin only)
    /// </summary>
    [HttpPut("{id}/status")]
    [Authorize]
    public async Task<ActionResult<object>> UpdateBookingStatus(int id, [FromBody] UpdateBookingStatusRequest request)
    {
        try
        {
            var booking = await _dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null)
                return NotFound(new { message = "Booking not found" });

            if (Enum.TryParse<BookingStatus>(request.Status, true, out var status))
            {
                booking.Status = status;
                booking.UpdatedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Booking {BookingId} status updated to {Status}", id, status);

                return Ok(new { message = "Booking status updated successfully" });
            }

            return BadRequest(new { message = "Invalid booking status" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking status");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}

public class CreateBookingRequest
{
    public int PropertyId { get; set; }
    public string GuestName { get; set; }
    public string GuestEmail { get; set; }
    public string GuestPhone { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public decimal TotalPrice { get; set; }
}

public class UpdateBookingStatusRequest
{
    public string Status { get; set; }
}
