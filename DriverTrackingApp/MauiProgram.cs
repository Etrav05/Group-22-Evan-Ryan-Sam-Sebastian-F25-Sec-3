using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;

namespace DriverTrackingApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            // CREATE THE MAUI APP BUILDER
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // REGISTER SERVICES AND PAGES
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "DriverTrackingData.sqlite");

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite($"Data Source={dbPath}");
            });

            // Transient services just means a new instance is created each time requested (Supposed to be good for small apps)
            builder.Services.AddTransient<TelemetryService>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<LoginPage>();

#if DEBUG  

#else

#endif

            // BUILD THE APP
            var app = builder.Build();

            // CREATE DATABASE + TABLES + BASE ACCOUNT AT STARTUP
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();  // make sure tables exist

                // Add a base account if none exist
                if (!db.AccountData.Any())
                {
                    var baseAccount = new Account
                    {
                        AccountID = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        password = "test123",
                        CreationDate = DateTime.UtcNow
                    };

                    db.AccountData.Add(baseAccount);
                    db.SaveChanges();
                    Console.WriteLine("Base account created - Yippy :)");
                }
            }

            return app;
        }
    }
}
