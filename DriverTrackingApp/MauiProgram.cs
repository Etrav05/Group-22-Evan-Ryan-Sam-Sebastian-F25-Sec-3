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
            var builder = MauiApp.CreateBuilder(); // Configs the app, services, pages, etc...
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => // These fonts can be used anywhere
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // REGISTER DATABASE, SERVICESAND PAGES
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "DriverTrackingData.sqlite"); // Builds the file path inside the devices local storage

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite($"Data Source={dbPath}"); // Register the SQLite database with the built path
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
            using (var scope = app.Services.CreateScope()) // Temp service scope so we can use the db context
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>(); // Gets the context sring
                db.Database.EnsureCreated();                                      // Makes sure tables exist, if not, creates them

                if (!db.AccountData.Any()) // If no accounts are found, create a base one
                {
                    var baseAccount = new Account
                    {
                        AccountID = Guid.Parse("11111111-1111-1111-1111-111111111111"), // Hardcoded atm
                        password = "pass123",
                        CreationDate = DateTime.UtcNow
                    };

                    db.AccountData.Add(baseAccount);  // Add and save the base account to that devices storage
                    db.SaveChanges();
                    Console.WriteLine("Base account created - Yippy :)");
                }
            }

            return app;
        }
    }
}
