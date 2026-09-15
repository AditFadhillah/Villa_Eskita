namespace VillaManager.Core.Entities;

/// <summary>
/// Represents a message from guests or booking platforms
/// </summary>
public class Message
{
    public int Id { get; set; }
    public int PropertyId { get; set; }
    public int? BookingId { get; set; }
    public string SenderName { get; set; }
    public string SenderPhone { get; set; }
    public string Content { get; set; }
    public MessageSource Source { get; set; } // WhatsApp, Airbnb, Agora
    public MessageType Type { get; set; } // Inquiry, Booking, Support
    public bool IsRead { get; set; } = false;
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    public string ExternalId { get; set; }
    
    // Navigation properties
    public Property Property { get; set; }
    public Booking Booking { get; set; }
}
