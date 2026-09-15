using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VillaManager.Core.Entities;
using VillaManager.Core.Enums;
using VillaManager.Infrastructure.Data;

namespace VillaManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly VillaManagerDbContext _dbContext;
    private readonly ILogger<MessageController> _logger;

    public MessageController(VillaManagerDbContext dbContext, ILogger<MessageController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Get unified inbox (Admin only)
    /// </summary>
    [HttpGet("inbox")]
    [Authorize]
    public async Task<ActionResult<List<object>>> GetInbox()
    {
        try
        {
            var messages = await _dbContext.Messages
                .OrderByDescending(m => m.ReceivedAt)
                .Select(m => new
                {
                    id = m.Id,
                    propertyId = m.PropertyId,
                    bookingId = m.BookingId,
                    senderName = m.SenderName,
                    senderPhone = m.SenderPhone,
                    content = m.Content,
                    source = m.Source.ToString(),
                    type = m.Type.ToString(),
                    isRead = m.IsRead,
                    receivedAt = m.ReceivedAt
                })
                .ToListAsync();

            return Ok(messages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting inbox");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get messages for a specific property
    /// </summary>
    [HttpGet("property/{propertyId}")]
    [Authorize]
    public async Task<ActionResult<List<object>>> GetPropertyMessages(int propertyId)
    {
        try
        {
            var messages = await _dbContext.Messages
                .Where(m => m.PropertyId == propertyId)
                .OrderByDescending(m => m.ReceivedAt)
                .Select(m => new
                {
                    id = m.Id,
                    propertyId = m.PropertyId,
                    bookingId = m.BookingId,
                    senderName = m.SenderName,
                    senderPhone = m.SenderPhone,
                    content = m.Content,
                    source = m.Source.ToString(),
                    type = m.Type.ToString(),
                    isRead = m.IsRead,
                    receivedAt = m.ReceivedAt
                })
                .ToListAsync();

            return Ok(messages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting property messages");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Mark a message as read (Admin only)
    /// </summary>
    [HttpPost("{id}/read")]
    [Authorize]
    public async Task<ActionResult<object>> MarkAsRead(int id)
    {
        try
        {
            var message = await _dbContext.Messages.FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
                return NotFound(new { message = "Message not found" });

            message.IsRead = true;
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Message marked as read" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking message as read");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get unread message count
    /// </summary>
    [HttpGet("unread-count")]
    [Authorize]
    public async Task<ActionResult<object>> GetUnreadCount()
    {
        try
        {
            var unreadCount = await _dbContext.Messages
                .CountAsync(m => !m.IsRead);

            return Ok(new { unreadCount });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting unread count");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Create inquiry message (from contact form)
    /// </summary>
    [HttpPost("inquiry")]
    [AllowAnonymous]
    public async Task<ActionResult<object>> CreateInquiry([FromBody] CreateInquiryRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var message = new Message
            {
                PropertyId = request.PropertyId,
                SenderName = request.SenderName,
                SenderPhone = request.SenderPhone,
                Content = request.Content,
                Source = MessageSource.Direct,
                Type = MessageType.Inquiry,
                IsRead = false,
                ReceivedAt = DateTime.UtcNow
            };

            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("New inquiry received from {SenderName}", request.SenderName);

            return CreatedAtAction(nameof(GetInbox), new
            {
                id = message.Id,
                message = "Thank you for your inquiry! We will get back to you soon."
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating inquiry");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}

public class CreateInquiryRequest
{
    public int PropertyId { get; set; }
    public string SenderName { get; set; }
    public string SenderPhone { get; set; }
    public string Content { get; set; }
}
