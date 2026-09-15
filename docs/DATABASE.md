# Villa Manager - Database Setup Guide

## Supabase Configuration

### 1. Create Supabase Project
- Visit https://supabase.com
- Sign in with GitHub or Google
- Click "New project"
- Name: VillaManager
- Region: Asia Southeast 1 (Singapore)
- Password: Your secure database password

### 2. Get Connection Credentials
Once project is created:
1. Go to Settings → Database → Connection string
2. Copy the "URI" connection string
3. Store in `.env` file

Your credentials:
```
Project ID: [YOUR_PROJECT_ID]
URL: [YOUR_SUPABASE_URL]
Anon Key: [YOUR_ANON_KEY]
Service Role Key: [YOUR_SERVICE_ROLE_KEY]
Database Password: [YOUR_PASSWORD]
```

### 3. Create Storage Bucket (for images)
1. Go to Storage in Supabase dashboard
2. Click "New bucket"
3. Name: `villa-images`
4. Make it public (for image serving)

## Entity Framework Core Migrations

### Initial Setup

```bash
# Navigate to backend
cd backend

# Add migration (creates migration files)
dotnet ef migrations add InitialCreate -p VillaManager.Infrastructure -s VillaManager.Api

# Apply migration to database (creates tables)
dotnet ef database update -p VillaManager.Infrastructure -s VillaManager.Api
```

### Database Tables Created

1. **properties**
   - Stores villa property information
   - One property per organization (single villa model)

2. **bookings**
   - Stores booking records from all platforms
   - Links to properties
   - Tracks booking status

3. **messages**
   - Unified inbox for all messages
   - Supports WhatsApp, Airbnb, Agora, and direct messages
   - Links to bookings and properties

### Indexes

For performance optimization, these indexes are automatically created:
- `bookings.property_id`
- `bookings.check_in_date`
- `bookings.status`
- `messages.property_id`
- `messages.booking_id`
- `messages.received_at DESC`
- `messages.is_read`

## Seed Data

When migrations are applied, a default property is seeded:
```json
{
  "id": 1,
  "name": "Your Villa Name",
  "description": "Beautiful villa in Indonesia with stunning views",
  "location": "Indonesia",
  "pricePerNight": 100.00,
  "maxGuests": 6,
  "imageUrls": ["villa1.jpg", "villa2.jpg", "villa3.jpg"]
}
```

Update this in appsettings after deployment.

## Backup & Recovery

### Backup from Supabase
1. Go to Supabase Dashboard
2. Settings → Backups
3. Backups are automatic (daily)

### Connect with pgAdmin
If you want to use pgAdmin for database management:

1. Get connection details from Supabase
2. Open pgAdmin
3. Register new server:
   - Host: `db.supabase.co`
   - Port: 5432
   - Username: postgres
   - Password: Your Supabase password
   - Database: postgres

## Connection Strings

### Development
```
postgresql://postgres:globalhopo@Eskita1@zjruplmkzijumnrtyblk.supabase.co:5432/postgres
```

### Environment Variables
```env
POSTGRES_URL=postgresql://postgres:globalhopo@Eskita1@zjruplmkzijumnrtyblk.supabase.co:5432/postgres
```

## Troubleshooting

### Migration Conflicts
```bash
# Remove last migration (if not applied)
dotnet ef migrations remove -p VillaManager.Infrastructure -s VillaManager.Api

# Recreate if needed
dotnet ef migrations add InitialCreate -p VillaManager.Infrastructure -s VillaManager.Api
```

### Connection Issues
1. Verify credentials in `.env`
2. Check firewall/network access
3. Test connection with psql:
   ```bash
   psql postgresql://postgres:password@host:5432/postgres
   ```

### Reset Database
```bash
# WARNING: This deletes all data
dotnet ef database drop -p VillaManager.Infrastructure -s VillaManager.Api

# Reapply migrations
dotnet ef database update -p VillaManager.Infrastructure -s VillaManager.Api
```

## Future Enhancements

- [ ] Add audit tables (who/when/what changed)
- [ ] Implement soft deletes
- [ ] Add read replicas for reports
- [ ] Implement data archiving for old bookings
- [ ] Add event sourcing for message history
