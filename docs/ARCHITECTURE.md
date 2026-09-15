# Villa Manager - Architecture Overview

## System Components

### Backend Architecture
```
VillaManager.Api (Entry point - ASP.NET Core Web API)
├── Controllers
│   ├── PropertyController
│   ├── BookingController
│   └── MessageController
└── Services (to be added)

VillaManager.Core (Business domain models)
├── Entities
│   ├── Property
│   ├── Booking
│   └── Message
└── Enums
    ├── BookingStatus
    ├── MessageSource
    └── MessageType

VillaManager.Infrastructure (Data access & external APIs)
├── Data
│   └── VillaManagerDbContext
└── Services (to be added)
    ├── WhatsAppService
    ├── AirbnbService
    └── AgoraService

VillaManager.Tests (Unit and integration tests)
```

### Frontend Architecture
```
frontend (Angular 18+ application)
├── src/
│   ├── app/
│   │   ├── admin/           (Admin dashboard)
│   │   │   ├── inbox/
│   │   │   ├── bookings/
│   │   │   └── guests/
│   │   ├── public/          (Customer-facing)
│   │   │   ├── home/
│   │   │   ├── gallery/
│   │   │   └── booking/
│   │   ├── shared/          (Shared components)
│   │   └── core/            (Services, interceptors)
│   ├── assets/
│   └── environments/
```

## Database Schema

### Properties Table
- id (int, PK)
- name (varchar)
- description (text)
- location (varchar)
- price_per_night (decimal)
- max_guests (int)
- image_urls (text array)
- created_at (timestamp)

### Bookings Table
- id (int, PK)
- property_id (int, FK)
- guest_name (varchar)
- guest_email (varchar)
- guest_phone (varchar)
- check_in_date (date)
- check_out_date (date)
- number_of_guests (int)
- total_price (decimal)
- status (varchar enum)
- external_booking_id (varchar)
- external_platform (varchar)
- created_at (timestamp)
- updated_at (timestamp)

### Messages Table
- id (int, PK)
- property_id (int, FK)
- booking_id (int, FK)
- sender_name (varchar)
- sender_phone (varchar)
- content (text)
- source (varchar enum)
- type (varchar enum)
- is_read (boolean)
- received_at (timestamp)
- external_id (varchar)

## API Endpoints

### Properties
- GET /api/property - Get all properties (public)
- GET /api/property/{id} - Get property details (public)

### Bookings
- GET /api/booking - List all bookings (admin)
- GET /api/booking/{id} - Get booking details (admin)
- POST /api/booking - Create booking inquiry (public)
- PUT /api/booking/{id}/status - Update booking status (admin)

### Messages
- GET /api/message/inbox - Get unified inbox (admin)
- GET /api/message/property/{id} - Get property messages (admin)
- POST /api/message/inquiry - Submit contact form (public)
- POST /api/message/{id}/read - Mark message as read (admin)
- GET /api/message/unread-count - Get unread count (admin)

## Technology Stack

### Backend
- .NET 10.0
- Entity Framework Core 10.0
- PostgreSQL (Supabase)
- AutoMapper
- Serilog
- JWT Authentication
- Swagger/OpenAPI

### Frontend
- Angular 18
- TypeScript
- SCSS
- Bootstrap 5
- RxJS

### Infrastructure
- PostgreSQL (Supabase)
- Supabase Storage (Images)
- Twilio WhatsApp API
- JWT for Authentication

## Development Workflow

1. **Backend Development**
   - Create entities and migrations
   - Implement services and controllers
   - Write unit tests
   - Document API endpoints

2. **Frontend Development**
   - Create components and services
   - Implement state management
   - Write component tests
   - Integrate with backend APIs

3. **Integration**
   - Connect frontend to backend
   - Test end-to-end flows
   - Performance optimization
   - Security review

## Security Considerations

- JWT-based authentication for admin panel
- CORS configuration for frontend access
- Input validation on all endpoints
- SQL injection prevention via EF Core
- Secure password storage (hashed)
- HTTPS enforcement in production
- API rate limiting (to be added)
