namespace VillaManager.Core.Entities;

/// <summary>
/// Represents a booking for a property
/// </summary>
public class Booking
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public string GuestName { get; set; }
    public string GuestEmail { get; set; }
    public string GuestPhone { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public string ExternalBookingId { get; set; }
    public string ExternalPlatform { get; set; } // "airbnb", "agora", "direct"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public Property Property { get; set; }
}
