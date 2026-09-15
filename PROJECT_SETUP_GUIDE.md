# Villa Management Platform - Full Stack Setup Guide

## 🎯 Project Vision

Build a **real-world SaaS platform** for managing villa bookings and guest communications:

### Customer Experience
- Beautiful booking website (Angular) showcasing the villa with photos
- Direct booking redirects to Airbnb/Agora
- Contact/inquiry form

### Admin Experience  
- **Unified inbox**: Consolidate WhatsApp, Airbnb, and Agora messages in one dashboard
- **Booking management**: View all bookings and guest information
- **Quick actions**: Reply to inquiries, mark bookings as confirmed/completed

### Technology
- **Backend**: ASP.NET Core with PostgreSQL
- **Frontend**: Angular 18+ (customer site + admin dashboard)
- **Storage**: Supabase (PostgreSQL + File Storage) or AWS S3
- **Messaging**: WhatsApp Business API integration
- **Single Property**: Optimized for one villa, but extensible to multiple

---

## 📋 Prerequisites

## 📋 Prerequisites

### Required Software
- **Node.js** 20.x or higher (LTS recommended)
- **.NET SDK** 9.0 or higher
- **Git** (for version control)
- **PostgreSQL** 15+ locally OR **Supabase** account (recommended for launch)
- **VS Code** with extensions:
  - C# Dev Kit
  - Angular Language Service
  - PostgreSQL
  - REST Client
  - Thunder Client (optional, for API testing)

### Required Accounts/Services
- **WhatsApp Business API** (for message integration)
- **Supabase** account (if using cloud PostgreSQL + file storage) — recommended
- **AWS S3** or **Azure Blob Storage** (alternative for file storage)
- **Airbnb Business Account** (to get Airbnb API access) — Phase 2
- **Agora** (Indonesian booking site) integration docs

### Optional
- **Docker Desktop** (for containerization)
- **Azure Tools** (if deploying to Azure)
- **Postman** or **Insomnia** (API testing)

---

## 📁 Project Structure Overview

```
VillaManager/
│
├── backend/
│   ├── VillaManager.Api/                # Main API project
│   ├── VillaManager.Core/               # Domain models, interfaces, business logic
│   ├── VillaManager.Infrastructure/     # Database, file storage, message APIs
│   ├── VillaManager.Tests/              # Unit tests
│   ├── VillaManager.sln                 # Solution file
│   ├── appsettings.json                 # Local configuration
│   ├── appsettings.Production.json      # Production configuration
│   └── Dockerfile
│
├── frontend/
│   ├── src/
│   │   ├── app/
│   │   │   ├── admin/                   # Admin dashboard
│   │   │   │   ├── inbox/              # Unified message inbox
│   │   │   │   ├── bookings/           # Booking management
│   │   │   │   └── guests/             # Guest list
│   │   │   ├── public/                 # Customer-facing site
│   │   │   │   ├── home/               # Hero section, gallery
│   │   │   │   ├── booking/            # Booking form/redirect
│   │   │   │   └── contact/            # Contact form
│   │   │   ├── shared/                 # Shared components
│   │   │   ├── core/                   # Services, auth
│   │   │   │   ├── services/
│   │   │   │   │   ├── api.service.ts
│   │   │   │   │   ├── auth.service.ts
│   │   │   │   │   └── message.service.ts
│   │   │   │   └── interceptors/
│   │   │   └── app.component.ts
│   │   ├── assets/
│   │   │   └── images/                 # Villa photos
│   │   ├── environments/
│   │   └── main.ts
│   ├── angular.json
│   ├── package.json
│   ├── tsconfig.json
│   └── Dockerfile
│
├── docs/
│   ├── DATABASE.md                      # Schema & migrations
│   ├── API.md                           # API endpoints
│   ├── WHATSAPP_SETUP.md               # WhatsApp integration guide
│   ├── DEPLOYMENT.md                    # Deployment procedures
│   └── ARCHITECTURE.md                  # Design decisions
│
├── docker-compose.yml                   # Local dev environment
├── .env.example                         # Environment variables template
├── .gitignore
├── README.md
└── PHASES.md                            # Phase 1, 2, 3 breakdown
```

---

## 🗄️ Cloud Storage Strategy

### Recommendation: **Start with Supabase** (Best for Launch)

**Why?**
- ✅ PostgreSQL built-in
- ✅ File storage (similar to S3) included
- ✅ Easy to scale
- ✅ Perfect for single property → multi-property migration
- ✅ One less vendor to manage

**Setup (5 mins):**
```bash
1. Create account at https://supabase.com
2. New project → Choose region closest to villa (Asia Southeast)
3. Get connection string (PostgreSQL + API keys)
4. Enable "Storage" bucket for villa photos
5. Store credentials in .env
```

**Cost**: Free tier covers MVP. $25/month for production.

### Alternative: **Hybrid Approach**
- **PostgreSQL**: Supabase (easier management)
- **File Storage**: AWS S3 (better image optimization)

---

## 🚀 Step-by-Step Setup Instructions

### Phase 0: Setup Supabase (Cloud Database)

#### Step 0.1: Create Supabase Project
```bash
# Go to https://supabase.com → Sign up
# Create new project
# Region: Singapore (closest to Indonesia)
# Save connection string and anon key to .env
```

#### Step 0.2: Create Environment File
```bash
# Create .env in project root
POSTGRES_URL="postgresql://postgres:[password]@db.supabase.co:5432/postgres"
SUPABASE_URL="https://[project-ref].supabase.co"
SUPABASE_ANON_KEY="[your-anon-key]"
SUPABASE_SERVICE_ROLE_KEY="[your-service-role-key]"
WHATSAPP_API_KEY="[will-get-later]"
JWT_SECRET="your-secret-key-here"
```

### Phase 1: Project Initialization

#### Step 1.1: Create Project Directory
```bash
mkdir VillaManager
cd VillaManager
git init
```

#### Step 1.2: Create Backend (.NET) Structure
```bash
# Create backend folder
mkdir backend
cd backend

# Create .NET solution
dotnet new sln -n VillaManager
dotnet new webapi -n VillaManager.Api -f net9.0
dotnet new classlib -n VillaManager.Core -f net9.0
dotnet new classlib -n VillaManager.Infrastructure -f net9.0
dotnet new xunit -n VillaManager.Tests -f net9.0

# Add projects to solution
dotnet sln add VillaManager.Api
dotnet sln add VillaManager.Core
dotnet sln add VillaManager.Infrastructure
dotnet sln add VillaManager.Tests

# Add project references
cd VillaManager.Api
dotnet add reference ../VillaManager.Core ../VillaManager.Infrastructure
cd ../VillaManager.Infrastructure
dotnet add reference ../VillaManager.Core
cd ../VillaManager.Tests
dotnet add reference ../VillaManager.Api ../VillaManager.Core ../VillaManager.Infrastructure
cd ../
```

#### Step 1.3: Return to Root and Create Frontend
```bash
cd ..
ng new frontend --routing --style=scss --skip-git --package-manager=npm
cd frontend
npm install
```

### Phase 2: Backend Configuration

#### Step 2.1: Essential NuGet Packages
```bash
cd backend/VillaManager.Api

# PostgreSQL & Database
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design

# Supabase client
dotnet add package Supabase

# WhatsApp integration
dotnet add package Twilio

# AutoMapper
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection

# Logging
dotnet add package Serilog
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File

# Validation
dotnet add package FluentValidation
dotnet add package FluentValidation.DependencyInjectionExtensions

# Security
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package System.IdentityModel.Tokens.Jwt

# API Documentation
dotnet add package Swashbuckle.AspNetCore

cd ../../
```

#### Step 2.2: Database Connection (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "postgresql://postgres:[password]@db.supabase.co:5432/postgres"
  },
  "Supabase": {
    "Url": "https://[project-ref].supabase.co",
    "AnonKey": "[your-anon-key]",
    "ServiceRoleKey": "[your-service-role-key]"
  },
  "WhatsApp": {
    "AccountSid": "[from-twilio]",
    "AuthToken": "[from-twilio]",
    "PhoneNumber": "[your-whatsapp-number]"
  },
  "Jwt": {
    "Key": "your-secret-key-min-32-characters-long",
    "Issuer": "VillaManager",
    "Audience": "VillaManagerApp"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

#### Step 2.3: Create Core Domain Models
```csharp
// VillaManager.Core/Entities/Property.cs
public class Property
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public decimal PricePerNight { get; set; }
    public int MaxGuests { get; set; }
    public string[] ImageUrls { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public ICollection<Booking> Bookings { get; set; }
    public ICollection<Message> Messages { get; set; }
}

// VillaManager.Core/Entities/Booking.cs
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
    public BookingStatus Status { get; set; } // Pending, Confirmed, Completed, Cancelled
    public string ExternalBookingId { get; set; } // AirBnB or Agora ID
    public string ExternalPlatform { get; set; } // "airbnb", "agora", "direct"
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public Property Property { get; set; }
}

// VillaManager.Core/Entities/Message.cs
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
    public bool IsRead { get; set; }
    public DateTime ReceivedAt { get; set; }
    public string ExternalId { get; set; } // For tracking original message
    
    public Property Property { get; set; }
    public Booking Booking { get; set; }
}

// VillaManager.Core/Enums
public enum BookingStatus
{
    Pending,
    Confirmed,
    CheckedIn,
    Completed,
    Cancelled
}

public enum MessageSource
{
    WhatsApp,
    Airbnb,
    Agora,
    Direct
}

public enum MessageType
{
    Inquiry,
    Booking,
    Support,
    Review
}
```

#### Step 2.4: Create DbContext
```csharp
// VillaManager.Infrastructure/Data/VillaManagerDbContext.cs
public class VillaManagerDbContext : DbContext
{
    public VillaManagerDbContext(DbContextOptions<VillaManagerDbContext> options)
        : base(options) { }

    public DbSet<Property> Properties { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Property configuration
        modelBuilder.Entity<Property>()
            .HasMany(p => p.Bookings)
            .WithOne(b => b.Property)
            .HasForeignKey(b => b.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Property>()
            .HasMany(p => p.Messages)
            .WithOne(m => m.Property)
            .HasForeignKey(m => m.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        // Message configuration
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Booking)
            .WithMany()
            .HasForeignKey(m => m.BookingId)
            .OnDelete(DeleteBehavior.SetNull);

        // Seed initial property
        var villaId = 1;
        modelBuilder.Entity<Property>().HasData(
            new Property
            {
                Id = villaId,
                Name = "Your Villa Name",
                Description = "Beautiful villa in Indonesia with stunning views",
                Location = "Indonesia",
                PricePerNight = 100m,
                MaxGuests = 6,
                ImageUrls = new[] { "image1.jpg", "image2.jpg", "image3.jpg" },
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}
```

#### Step 2.5: Database Migration
```bash
cd backend

# Create migrations
dotnet ef migrations add InitialCreate -p VillaManager.Infrastructure -s VillaManager.Api

# Update database (Supabase)
dotnet ef database update -p VillaManager.Infrastructure -s VillaManager.Api
```

#### Step 2.6: Create Program.cs
```csharp
// VillaManager.Api/Program.cs
var builder = WebApplicationBuilder.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<VillaManagerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Repositories & Services
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IWhatsAppService, WhatsAppService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();

// CORS - Important for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", corsBuilder =>
        corsBuilder.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"])),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true
        };
    });

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

#### Step 2.7: Create Core API Endpoints

```csharp
// VillaManager.Api/Controllers/PropertyController.cs
[ApiController]
[Route("api/[controller]")]
public class PropertyController : ControllerBase
{
    private readonly IPropertyService _propertyService;

    public PropertyController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<PropertyDto>> GetProperty(int id)
    {
        var property = await _propertyService.GetPropertyAsync(id);
        if (property == null)
            return NotFound();
        return Ok(property);
    }
}

// VillaManager.Api/Controllers/BookingController.cs
[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookings()
    {
        var bookings = await _bookingService.GetAllBookingsAsync();
        return Ok(bookings);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<BookingDto>> CreateBooking(CreateBookingDto dto)
    {
        var booking = await _bookingService.CreateBookingAsync(dto);
        return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<BookingDto>> GetBooking(int id)
    {
        var booking = await _bookingService.GetBookingAsync(id);
        if (booking == null)
            return NotFound();
        return Ok(booking);
    }
}

// VillaManager.Api/Controllers/MessageController.cs
[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet("inbox")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetInbox()
    {
        var messages = await _messageService.GetInboxAsync();
        return Ok(messages);
    }

    [HttpPost("{id}/reply")]
    [Authorize]
    public async Task<ActionResult> ReplyToMessage(int id, ReplyMessageDto dto)
    {
        await _messageService.ReplyToMessageAsync(id, dto);
        return Ok(new { message = "Reply sent successfully" });
    }

    [HttpPost("{id}/read")]
    [Authorize]
    public async Task<ActionResult> MarkAsRead(int id)
    {
        await _messageService.MarkAsReadAsync(id);
        return Ok();
    }

    [HttpPost("webhook/whatsapp")]
    [AllowAnonymous]
    public async Task<ActionResult> ReceiveWhatsAppMessage(WhatsAppWebhookDto dto)
    {
        // Handle incoming WhatsApp message
        await _messageService.HandleWhatsAppWebhookAsync(dto);
        return Ok();
    }
}
```

### Phase 3: Frontend Configuration

#### Step 3.1: Angular Project Structure
```bash
cd frontend

# Generate modules
ng generate module core
ng generate module shared
ng generate module admin
ng generate module public

# Generate services
ng generate service core/services/api
ng generate service core/services/auth
ng generate service core/services/message
ng generate service core/services/property

# Generate components
ng generate component admin/inbox
ng generate component admin/bookings
ng generate component public/home
ng generate component public/gallery
ng generate component public/booking-form
```

#### Step 3.2: Install Frontend Dependencies
```bash
npm install @ng-bootstrap/ng-bootstrap bootstrap
npm install ngx-toastr
npm install ngx-spinner
npm install date-fns
npm install qrcode
```

#### Step 3.3: API Service
```typescript
// frontend/src/app/core/services/api.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  // Properties
  getProperty(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/property/${id}`);
  }

  // Bookings
  getBookings(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/booking`);
  }

  createBooking(booking: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/booking`, booking);
  }

  // Messages
  getInbox(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/message/inbox`);
  }

  replyToMessage(messageId: number, reply: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/message/${messageId}/reply`, reply);
  }

  markAsRead(messageId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/message/${messageId}/read`, {});
  }
}
```

#### Step 3.4: Environment Configuration
```typescript
// frontend/src/environments/environment.ts
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7001/api',
  bookingRedirectUrl: 'https://www.airbnb.com/rooms/[property-id]'
};

// frontend/src/environments/environment.prod.ts
export const environment = {
  production: true,
  apiUrl: 'https://api.yourvilla.com/api',
  bookingRedirectUrl: 'https://www.airbnb.com/rooms/[property-id]'
};
```

### Phase 4: Running the Applications

#### Step 4.1: Start Backend
```bash
cd backend/VillaManager.Api
dotnet run
# API runs at https://localhost:7001
# Swagger UI: https://localhost:7001/swagger
```

#### Step 4.2: Start Frontend (in new terminal)
```bash
cd frontend
ng serve
# Frontend runs at http://localhost:4200
```

---

## 📊 Phase 1: Core Features (MVP)

### ✅ Customer Website
- [ ] Home page with hero section
- [ ] Photo gallery (upload via admin, display on frontend)
- [ ] Property details (description, amenities, pricing)
- [ ] Booking inquiry form (stores in database, sends notification)
- [ ] Contact form
- [ ] Responsive mobile design

### ✅ Admin Dashboard
- [ ] Admin login (JWT authentication)
- [ ] Unified message inbox (WhatsApp messages stored)
- [ ] Message view/reply interface
- [ ] Booking list view
- [ ] Guest contact information

### ✅ Backend APIs
- [ ] GET /api/property/{id} — Public property details
- [ ] GET /api/booking — Admin: list all bookings
- [ ] POST /api/booking — Public: create booking inquiry
- [ ] GET /api/message/inbox — Admin: unified inbox
- [ ] POST /api/message/{id}/reply — Admin: send reply via WhatsApp
- [ ] POST /api/message/webhook/whatsapp — Receive WhatsApp webhooks

---

## 📊 Phase 2: Enhanced Features

### ✅ Message Consolidation
- [ ] Airbnb API integration (fetch bookings & messages)
- [ ] Agora integration (fetch bookings)
- [ ] Message tagging (inquiry, booking, support)
- [ ] Message search & filter

### ✅ Booking Management
- [ ] Calendar view (booked dates)
- [ ] Booking status workflow (Pending → Confirmed → Completed)
- [ ] Automated notifications to guests
- [ ] Booking revenue tracking

### ✅ Image Management
- [ ] Upload villa photos to Supabase Storage
- [ ] Photo gallery display on homepage
- [ ] Lightbox/carousel view

---

## 🔐 WhatsApp Integration Setup

### Step 1: Create Twilio Account
```bash
1. Go to https://www.twilio.com
2. Sign up and verify phone number
3. Get Account SID and Auth Token
4. Enable WhatsApp API
5. Save credentials to .env
```

### Step 2: Backend WhatsApp Service
```csharp
// VillaManager.Infrastructure/Services/WhatsAppService.cs
public interface IWhatsAppService
{
    Task SendMessageAsync(string phoneNumber, string message);
    Task<WhatsAppMessage> ProcessWebhookAsync(Dictionary<string, string> webhook);
}

public class WhatsAppService : IWhatsAppService
{
    private readonly TwilioClient _twilioClient;
    private readonly IMessageRepository _messageRepository;

    public WhatsAppService(IOptions<WhatsAppSettings> options, IMessageRepository messageRepository)
    {
        TwilioClient.Init(options.Value.AccountSid, options.Value.AuthToken);
        _messageRepository = messageRepository;
    }

    public async Task SendMessageAsync(string phoneNumber, string message)
    {
        var messageResource = await MessageResource.CreateAsync(
            from: new PhoneNumber($"whatsapp:{options.Value.PhoneNumber}"),
            to: new PhoneNumber($"whatsapp:{phoneNumber}"),
            body: message
        );
    }

    public async Task<WhatsAppMessage> ProcessWebhookAsync(Dictionary<string, string> webhook)
    {
        // Extract message data from webhook
        var message = new WhatsAppMessage
        {
            SenderPhone = webhook["From"],
            Content = webhook["Body"],
            ExternalId = webhook["MessageSid"],
            ReceivedAt = DateTime.UtcNow,
            Source = MessageSource.WhatsApp
        };

        await _messageRepository.AddAsync(message);
        return message;
    }
}
```

### Step 3: Configure Webhook in Twilio
```
Webhook URL: https://yourapi.com/api/message/webhook/whatsapp
Method: POST
```

---

## 🚀 Running Your Project

```bash
# Terminal 1: Backend
cd backend/VillaManager.Api
dotnet run

# Terminal 2: Frontend
cd frontend
ng serve

# Open browser
# Customer site: http://localhost:4200
# Admin dashboard: http://localhost:4200/admin
# Swagger API: https://localhost:7001/swagger
```

---

## 📂 Database Schema (PostgreSQL)

```sql
-- Properties table
CREATE TABLE properties (
  id SERIAL PRIMARY KEY,
  name VARCHAR(255) NOT NULL,
  description TEXT,
  location VARCHAR(255),
  price_per_night DECIMAL(10, 2),
  max_guests INT,
  image_urls TEXT[],
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Bookings table
CREATE TABLE bookings (
  id SERIAL PRIMARY KEY,
  property_id INT NOT NULL REFERENCES properties(id) ON DELETE CASCADE,
  guest_name VARCHAR(255) NOT NULL,
  guest_email VARCHAR(255),
  guest_phone VARCHAR(20),
  check_in_date DATE NOT NULL,
  check_out_date DATE NOT NULL,
  number_of_guests INT,
  total_price DECIMAL(10, 2),
  status VARCHAR(50),
  external_booking_id VARCHAR(255),
  external_platform VARCHAR(50),
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  updated_at TIMESTAMP
);

-- Messages table
CREATE TABLE messages (
  id SERIAL PRIMARY KEY,
  property_id INT NOT NULL REFERENCES properties(id) ON DELETE CASCADE,
  booking_id INT REFERENCES bookings(id) ON DELETE SET NULL,
  sender_name VARCHAR(255),
  sender_phone VARCHAR(20),
  content TEXT NOT NULL,
  source VARCHAR(50),
  type VARCHAR(50),
  is_read BOOLEAN DEFAULT FALSE,
  received_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  external_id VARCHAR(255)
);

-- Indexes for performance
CREATE INDEX idx_bookings_property ON bookings(property_id);
CREATE INDEX idx_messages_property ON messages(property_id);
CREATE INDEX idx_messages_booking ON messages(booking_id);
CREATE INDEX idx_messages_received_at ON messages(received_at DESC);
```

---

## 📚 Additional Resources & Next Steps

### Immediate Tasks
1. [ ] Set up Supabase account and PostgreSQL
2. [ ] Get WhatsApp Business API access (Twilio)
3. [ ] Gather villa photos and organize
4. [ ] Create content (description, amenities)
5. [ ] Design admin dashboard wireframes

### Phase 2 Tasks
1. [ ] Airbnb API integration documentation
2. [ ] Agora booking platform integration
3. [ ] Calendar view implementation
4. [ ] Automated email/SMS notifications

### Phase 3 (Future)
1. [ ] Payment integration (Stripe)
2. [ ] Multi-property support
3. [ ] Revenue analytics
4. [ ] Mobile app (MAUI)

---

## 🔗 Useful Links

- **Supabase Docs**: https://supabase.com/docs
- **Twilio WhatsApp**: https://www.twilio.com/whatsapp
- **Airbnb API**: https://www.airbnb.com/partner
- **Angular Docs**: https://angular.io/docs
- **ASP.NET Core**: https://docs.microsoft.com/en-us/aspnet/core

---

**Let's build this! 🚀**

