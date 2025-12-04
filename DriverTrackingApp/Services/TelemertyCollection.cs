using Microsoft.EntityFrameworkCore.Diagnostics;

public class TelemetryService
{
    private readonly AppDbContext _db;    // Inject the database context
    public Guid AccountId { get; set; }

    private Guid _currentTripId;         // Track the current trip by its guid
    private double _AccelMagnitude = 0; // Track acceleration accross a whole trip
    private List<TripDataPoints> _buffer = new List<TripDataPoints>();

    public TelemetryService(AppDbContext db)
    {
        _db = db;
        AccountId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    }

    public async Task StartAsync()
    {
        try
        {
            PermissionStatus status = PermissionStatus.Unknown;

            await MainThread.InvokeOnMainThreadAsync(async () => // Make sure permission requrest are always on the main thread
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            });

            if (status != PermissionStatus.Granted)
            {
                Console.WriteLine("Location permission NOT granted :(");
                return;
            }

            _currentTripId = await CreateTripAsync();

            _ = StartLocationCollection();
            StartAccelerationCollection();
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR in StartAsync: " + ex);
        }
    }

    private async Task<Guid> CreateTripAsync()
    {
        Console.WriteLine("ACCOUNT ID = " + AccountId);
        var trip = new Trips
        {
            AccountID = AccountId,
            StartTime = DateTime.UtcNow
        };

        _db.TripData.Add(trip);
        await _db.SaveChangesAsync();

        return trip.TripID; // EF auto-fills this ID
    }

    // Speed is also found in the location data
    private async Task StartLocationCollection()
    {
        Console.WriteLine("Telemetry started!");

        var request = new GeolocationRequest(GeolocationAccuracy.High);

        while (true)
        {
            var location = await Geolocation.GetLocationAsync(request);

            Console.WriteLine("awaiting point");

            if (location != null)
            {
                var tdpoint = new TripDataPoints
                {
                    TripID = _currentTripId,
                    AccountID = AccountId,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    Speed = location.Speed ?? 0,
                    Acceleration = _AccelMagnitude,
                    TimeData = DateTime.UtcNow
                };

                Console.WriteLine(tdpoint);

                Console.WriteLine($"TripID={_currentTripId}, AccountID={AccountId}"); // DEBUG
                _buffer.Add(tdpoint);

                if (_buffer.Count >= 10)
                {
                    _db.TripDataPoints.AddRange(_buffer);
                    await _db.SaveChangesAsync();

                    _buffer.Clear();
                }
            }

            await Task.Delay(1000); // Only read every second (This is in milliseconds)
        }
    }

    public async Task StopAsync()
    {
        Accelerometer.Stop();

        if (_buffer.Count > 0)
        {
            _db.TripDataPoints.AddRange(_buffer);
            await _db.SaveChangesAsync();
            _buffer.Clear();
        }

        Console.WriteLine("Telemetry stopped!");
    }

    private void StartAccelerationCollection()
    {
        Accelerometer.ReadingChanged += (sender, eventData) =>
        {

            var x = eventData.Reading.Acceleration.X;
            var y = eventData.Reading.Acceleration.Y;
            var z = eventData.Reading.Acceleration.Z;

            _AccelMagnitude = Math.Sqrt(x * x + y * y + z * z); // Simple magnitude formula (Euclidean Norm)

            Console.WriteLine($"Acceleration: {x}, {y}, {z}");
        };

        Accelerometer.Start(SensorSpeed.UI);
    }
}