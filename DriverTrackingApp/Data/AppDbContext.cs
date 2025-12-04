using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Animations;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Account> AccountData { get; set; }
    public DbSet<Trips> TripData { get; set; }
    public DbSet<TripDataPoints> TripDataPoints { get; set; }
    public DbSet<DriverScore> DriverScores { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
     : base(options)
    {
    }
}