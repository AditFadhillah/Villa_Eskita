namespace VillaManager.Core.Entities;

/// <summary>
/// Represents a villa property
/// </summary>
public class Property
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public decimal PricePerNight { get; set; }
    public int MaxGuests { get; set; }
    public string[] ImageUrls { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ICollection<Booking> Bookings { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
}
