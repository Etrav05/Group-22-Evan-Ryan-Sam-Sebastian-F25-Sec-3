using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SADMyProjectTemplate.API.Models;

// This is going to be our database session/gateway
// Whenever this app wants to query, insert, update, or delete data, it does it through this class
namespace SADMyProjectTemplate.API.Data
{
    public class AppDbContext : DbContext // Inherits from DbContext, this is the main EF Core class responsible for both managing database connections and tracking data changes
    {
        // Basically a config package that tells our EF Core how to connect to the database
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // This class tells EF core to create a table for DriverData in the DB and lets us query/save DriverData objects using this property
        // The API is jnow ready for the collected driver data
        public DbSet<DriverData> DriverData { get; set; }  // example usage: _db.DriverData.Add(new DriverData { Speed = 50 }); 
    }
}