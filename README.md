# Villa Manager - Full Stack SaaS Platform

A comprehensive property management and booking system for villa rental businesses. Consolidates bookings and messages from multiple platforms (WhatsApp, Airbnb, Agora) into a unified admin dashboard.

## 🎯 Project Overview

- **Backend**: ASP.NET Core 10.0 Web API with PostgreSQL
- **Frontend**: Angular 18+ with responsive design
- **Database**: Supabase (PostgreSQL + File Storage)
- **Messaging**: WhatsApp Business API integration

### Key Features (MVP)
- ✅ Unified message inbox (WhatsApp, Airbnb, Agora, Direct)
- ✅ Booking management dashboard
- ✅ Guest information tracking
- ✅ Public villa showcase website
- ✅ Booking inquiry form
- ✅ Admin authentication (JWT)
- ✅ RESTful API with Swagger documentation

## 🚀 Quick Start

### Prerequisites
- .NET SDK 10.0+
- Node.js 20+ and npm
- Git
- Supabase account (free tier available)

### 1. Environment Setup

```bash
# Copy environment file
cp .env.example .env

# Edit .env with your credentials
# Your Supabase credentials are already configured
```

### 2. Backend Setup

```bash
cd backend

# Install dependencies (already done)
# dotnet restore

# Run migrations (auto-runs on startup, but can be manual)
dotnet ef database update -p VillaManager.Infrastructure -s VillaManager.Api

# Run API
dotnet run --project VillaManager.Api/VillaManager.Api.csproj

# API runs at https://localhost:7001
# Swagger UI: https://localhost:7001/swagger
```

### 3. Frontend Setup

```bash
cd frontend

# Install dependencies (already done)
# npm install

# Run development server
npm start

# Frontend runs at http://localhost:4200
```

## 📁 Project Structure

```
Villa_Eskita/
├── backend/
│   ├── VillaManager.Api/          # ASP.NET Core API
│   ├── VillaManager.Core/         # Domain models & entities
│   ├── VillaManager.Infrastructure/  # Database context & repos
│   ├── VillaManager.Tests/        # Unit tests
│   └── VillaManager.sln           # Solution file
│
├── frontend/
│   ├── src/
│   │   ├── app/
│   │   │   ├── admin/             # Admin dashboard
│   │   │   ├── public/            # Customer-facing site
│   │   │   ├── shared/            # Shared components
│   │   │   └── core/              # Services & auth
│   │   └── environments/          # Config by environment
│   ├── package.json
│   └── angular.json
│
├── docs/
│   ├── ARCHITECTURE.md            # System design
│   ├── DATABASE.md                # Database setup
│   └── API.md                     # API documentation (coming)
│
├── .env                           # Environment variables (local)
├── .env.example                   # Environment template
├── .gitignore                     # Git ignore rules
└── README.md                      # This file
```

## 🔐 Configuration

### Supabase Credentials
```
Project ID: zjruplmkzijumnrtyblk
API URL: https://zjruplmkzijumnrtyblk.supabase.co
Database: postgresql://postgres:***@db.supabase.co:5432/postgres
```

### Environment Variables
Located in `.env`:
- `SUPABASE_URL` - Supabase project URL
- `SUPABASE_ANON_KEY` - Public API key
- `SUPABASE_SERVICE_ROLE_KEY` - Service role key
- `POSTGRES_URL` - Database connection string
- `JWT_SECRET` - Secret for JWT token generation
- `CORS_ALLOWED_ORIGINS` - Frontend URLs

## 🗄️ Database

### Tables
1. **properties** - Villa property details
2. **bookings** - All booking records
3. **messages** - Unified message inbox

### Migrations
```bash
# Create new migration
dotnet ef migrations add MigrationName -p VillaManager.Infrastructure -s VillaManager.Api

# Apply migrations
dotnet ef database update -p VillaManager.Infrastructure -s VillaManager.Api

# Revert last migration
dotnet ef migrations remove -p VillaManager.Infrastructure -s VillaManager.Api
```

## 📡 API Endpoints

### Public Endpoints
```
GET    /api/property              - List properties
GET    /api/property/{id}         - Get property details
POST   /api/booking               - Create booking inquiry
POST   /api/message/inquiry       - Submit contact form
```

### Admin Endpoints (Requires Authentication)
```
GET    /api/booking               - List all bookings
GET    /api/booking/{id}          - Get booking details
PUT    /api/booking/{id}/status   - Update booking status
GET    /api/message/inbox         - Unified inbox
GET    /api/message/unread-count  - Unread message count
POST   /api/message/{id}/read     - Mark message as read
```

## 🧪 Testing

### Run Tests
```bash
cd backend
dotnet test
```

### Test Coverage
- Unit tests for services
- Integration tests for API endpoints
- (To be added) E2E tests for workflows

## 🔧 Development Workflow

### Add a New Feature

1. **Backend**
   ```bash
   cd backend
   # Create entity in VillaManager.Core/Entities/
   # Create migration
   dotnet ef migrations add FeatureName -p VillaManager.Infrastructure -s VillaManager.Api
   # Create controller in VillaManager.Api/Controllers/
   # Create tests
   dotnet test
   ```

2. **Frontend**
   ```bash
   cd frontend
   # Create component
   ng generate component feature/component-name
   # Create service
   ng generate service services/service-name
   # Test locally
   npm start
   ```

3. **Integration**
   - Connect frontend service to backend API
   - Update environment URLs
   - Test full flow

## 📚 Documentation

- [Architecture](docs/ARCHITECTURE.md) - System design and components
- [Database](docs/DATABASE.md) - Schema and migrations
- [API](docs/API.md) - Endpoint documentation (coming soon)

## 🚢 Deployment

### Development
```bash
# Run both simultaneously in separate terminals

# Terminal 1
cd backend && dotnet run --project VillaManager.Api/VillaManager.Api.csproj

# Terminal 2
cd frontend && npm start
```

### Production
- Deploy backend to Azure App Service / AWS EC2 / Heroku
- Deploy frontend to Azure Static Web Apps / Netlify / Vercel
- Database: Supabase (automatically managed)
- File storage: Supabase Storage or AWS S3

## 🛠️ Troubleshooting

### Port Already in Use
```bash
# Find process using port 7001
lsof -i :7001
# Kill it
kill -9 <PID>
```

### Database Connection Issues
```bash
# Test connection
psql postgresql://postgres:password@db.supabase.co:5432/postgres

# Check .env credentials
cat .env
```

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

## 📄 License

This project is proprietary software. All rights reserved.

---

**Happy coding! 🚀**

**Next Steps:**
1. ✅ Project structure created
2. ✅ Backend configured with .NET, EF Core, PostgreSQL
3. ✅ Frontend initialized with Angular 18
4. ✅ Environment variables configured
5. ⏭️ Run backend: `dotnet run --project backend/VillaManager.Api/VillaManager.Api.csproj`
6. ⏭️ Run frontend: `npm start` (from frontend folder)
7. ⏭️ Create authentication system
8. ⏭️ Build admin dashboard UI
9. ⏭️ Integrate WhatsApp API
