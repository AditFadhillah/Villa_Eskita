# Villa Manager - Full Stack SaaS Platform (Work in Progress)

A property management and booking system for villa rental businesses. Still figuring out how to consolidate bookings and messages from multiple platforms (WhatsApp, Airbnb, Agora) into a unified admin dashboard. Not fully implemented yet.

## Project Overview

- Backend: ASP.NET Core 10.0 Web API with PostgreSQL (mostly set up)
- Frontend: Angular 18+ with responsive design (basic setup done)
- Database: Supabase (PostgreSQL + File Storage)
- Messaging: WhatsApp Business API integration (planned)

### Features (Partially Implemented)
- Unified message inbox (WhatsApp, Airbnb, Agora, Direct) - not done
- Booking management dashboard - UI started, backend incomplete
- Guest information tracking - schema exists, needs work
- Public villa showcase website - basically working
- Booking inquiry form - rough implementation
- Admin authentication (JWT) - started but not tested
- RESTful API with Swagger documentation - auto-generated

## Quick Start (If You Want to Try It)

### What You Need
- .NET SDK 10.0 or later
- Node.js 20+ with npm
- Git
- Supabase account (have one set up already)

### Setup Steps (These Should Work, Probably)

```bash
# Copy the environment file
cp .env.example .env

# Edit .env with your credentials
# Supabase stuff is already in there somewhere
```

#### Backend
```bash
cd backend

# Dependencies should already be restored

# Run migrations (hopefully)
dotnet ef database update -p VillaManager.Infrastructure -s VillaManager.Api

# Start the API
dotnet run --project VillaManager.Api/VillaManager.Api.csproj

# Should be at https://localhost:7001
# Swagger docs at https://localhost:7001/swagger (if it's working)
```

#### Frontend
```bash
cd frontend

# Dependencies should already be installed

# Start dev server
npm start

# Should open at http://localhost:4200
```

## 📁 Project Structure

```
Villa_Eskita/
├──Project Structure (Sort of Organized)

```
Villa_Eskita/
├── backend/
│   ├── VillaManager.Api/          # ASP.NET Core API (main entry point)
│   ├── VillaManager.Core/         # Domain models and entities
│   ├── VillaManager.Infrastructure/  # Database stuff, repositories
│   ├── VillaManager.Tests/        # Tests (need more)
│   └── VillaManager.sln           # The solution file
│
├── frontend/
│   ├── src/
│   │   ├── app/
│   │   │   ├── admin/             # Admin dashboard (work in progress)
│   │   │   ├── public/            # Customer site (mostly done)
│   │   │   ├── shared/            # Reusable components
│   │   │   └── core/              # Services, auth stuff
│   │   └── environments/          # Environment config
│   ├── package.json
│   └── angular.json
│
├── docs/
│   ├── ARCHITECTURE.md            # Design notes (incomplete)
│   ├── DATABASE.md                # Database stuff
│   └── API.md                     # API docs (not written yet)
│
├── .env                           # Local config
├── .env.example                   # Template for .env
├──Configuration (Credentials Hidden)

### Supabase
Some Supabase stuff is configured, but I'm not listing the credentials here for security reasons.

### Environment Variables
Located in `.env`:
- SUPABASE_URL
- SUPABASE_ANON_KEY
- SUPABASE_SERVICE_ROLE_KEY
- POSTGRES_URL
- JWT_SECRET (needs to be set properly)
- CORS_ALLOWED_ORIGINS

## 🗄️ Database

### Tables
1. **properties** - Villa property details
2. Database (Incomplete)

### Tables (Probably)
- properties - villa details (schema defined)
- bookings - booking records (partially implemented)
- messages - message inbox (not connected yet)

### Migrations
```bash
# Create atabase update -p VillaManager.Infrastructure -s VillaManager.Api

# Revert last migration
dotnet ef migrations remove -p VillaManager.Infrastructure -s VillaManager.Api
```

## 📡 API Endpoints

### Public Endpoints
```
GETAPI Endpoints (These Might Work)

### Public Endpoints (Probably Working)
```
GET    /api/property              - List properties (untested)
GET    /api/property/{id}         - Get property details (untested)
POST   /api/booking               - Create booking inquiry (rough)
POST   /api/message/inquiry       - Contact form (basic)
```

### Admin Endpoints (Need Work)
```
GET    /api/booking               - List bookings (not connected)
GET    /api/booking/{id}          - Get booking details (not connected)
PUT    /api/booking/{id}/status   - Update status (not implemented)
GET    /api/message/inbox         - Unified inbox (not done)
GETTesting (Needs Work)

```bash
cd backend
dotnet test
```

Tests exist for some stuff, but coverage is limited. Need to add more.

## 🔧 Development Workflow

### Add a New Feature

1. Development Workflow (Sort Of)

### Adding a Feature

1. Backend
   ```bash
   cd backend
   # Create entity in VillaManager.Core/Entities/
   # Create migration
   dotnet ef migrations add FeatureName -p VillaManager.Infrastructure -s VillaManager.Api
   # Add controller in VillaManager.Api/Controllers/
   # Write tests (if you remember)
   dotnet test
   ```

2. Frontend
   ```bash
   cd frontend
   # Create component
   ng generate component feature/component-name
   # Create service
   ng generate service services/service-name
   # Test locally
   npm start
   ```

3. Integration
   - Connect frontend service to backend API
   - Update environment URLs
   - Test it (hopefully)cs/ARCHITECTURE.md) - System design and components
- [Database](docs/DATABASE.md) - Schema and migrations
- [Documentation

- [Architecture](docs/ARCHITECTURE.md) - System design (incomplete)
- [Database](docs/DATABASE.md) - Database info
- [API](docs/API.md) - API docs (not written yet
```bash
# RDeployment (Not Ready)

### Development
```bash
# Run both in separate terminals

# Terminal 1
cd backend && dotnet run --project VillaManager.Api/VillaManager.Api.csproj

# Terminal 2
cd frontend && npm start
```

### Production
- Not ready for production yet
- CTroubleshooting (Maybe)

### Port Already in Use
```bash
# Find process on port 7001
lsof -i :7001
# Kill it
kill -9 <PID>
```

### Database Connection Issues
Check if your .env file has the right Supabase credentials.

### Frontend Build Errors
```bash
cd frontend
npm install
npm run build
```

## 📞 Support & Contact

For issues and feature requests:
- Create an issue in GitHub
- Contact: [your-email@example.com]

## Contact

For questions or issues:
- Create an issue on GitHub
- Or email [add email later]

## License

Proprietary software. All rights reserved (or something like that).

---

## What Still Needs to Happen

1. Set up backend database migrations properly
2. Build admin dashboard UI (partially started)
3. Connect message inbox to WhatsApp API (not started)
4. Test authentication flow
5. Integrate Airbnb and Agora APIs (planned but not started)
6. Write actual tests
7. Fix various bugs that probably exist
8. Deploy somewhere (not urgent)
9. Document the API properly
10. Handle all the edge cases nobody thought of yet