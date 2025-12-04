using DriverTrackingApp.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Threading;

public class TelemetryService
{
    private readonly AppDbContext _db;       // Inject the database context
    private readonly ProcessTrip _processTrip;
    public Guid AccountId { get; set; }

    private Guid _currentTripId;         // Track the current trip by its guid
    private double _AccelMagnitude = 0; // Track acceleration accross a whole trip
    private List<TripDataPoints> _buffer = new List<TripDataPoints>();

    private CancellationTokenSource _cancellationTokenSource; // Function from the "Task Parallel Library" (TPL) which ends async tasks 

    public TelemetryService(AppDbContext db, ProcessTrip processTrip)
    {
        _db = db;
        _processTrip = processTrip;

        AccountId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    }

    public async Task StartAsync() // Task = async thing that can run without stopping the program
    {
        try
        {
            PermissionStatus status = PermissionStatus.Unknown; // Track if we have permission yet

            await MainThread.InvokeOnMainThreadAsync(async () => // Make sure permission requrest are always on the main thread
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>(); // This is a that location popup I showed you guys
            });

            if (status != PermissionStatus.Granted)
            {
                Console.WriteLine("Location permission NOT granted :(");
                return;
            }

            _currentTripId = await CreateTripAsync(); // Start a new trip and get its ID

            _cancellationTokenSource = new CancellationTokenSource();

            _ = StartLocationCollection(_cancellationTokenSource.Token);      // Begin collecting location data (fire and forget)
            _ = StartAccelerationCollection(_cancellationTokenSource.Token); // Same here
        }

        catch (Exception ex) // Kept getting errors with permissions, so added this to catch them
        {
            Console.WriteLine("ERROR in StartAsync: " + ex);
        }
    }

    private async Task<Guid> CreateTripAsync()
    {
        Console.WriteLine("ACCOUNT ID = " + AccountId); // DEBUG
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
    private async Task StartLocationCollection(CancellationToken cancellationToken)
    {
        Console.WriteLine("Telemetry started!"); // DEBUG

        var request = new GeolocationRequest(GeolocationAccuracy.High);

        while (!cancellationToken.IsCancellationRequested)
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

                Console.WriteLine($"TripDataPoint: Lng - {tdpoint.Longitude}, Lat - {tdpoint.Latitude}, Spd - {tdpoint.Speed}"); // DEBUG
                Console.WriteLine($"TripID={_currentTripId}, AccountID={AccountId}");                           // DEBUG

                _buffer.Add(tdpoint);

                if (_buffer.Count >= 10)
                {
                    _db.TripDataPoints.AddRange(_buffer);
                    await _db.SaveChangesAsync();
                    Console.WriteLine("DataPoint buffer saved");

                    _buffer.Clear();
                }
            }

            await Task.Delay(1000, cancellationToken); // Only read every second (This is in milliseconds)
        }
    }

    public async Task StopAsync()
    {
        _cancellationTokenSource?.Cancel();

        Accelerometer.Stop();

        if (_buffer.Count > 0)
        {
            _db.TripDataPoints.AddRange(_buffer);
            await _db.SaveChangesAsync();
            _buffer.Clear();
        }
         
        await _processTrip.ProcessTripSummary(_currentTripId); // Process the trip summary

        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;

        Console.WriteLine("Telemetry stopped!");
    }

    private async Task StartAccelerationCollection(CancellationToken cancellationToken)
    {
        try
        {
            Accelerometer.ReadingChanged += (sender, eventData) =>
            {
                var x = eventData.Reading.Acceleration.X;
                var y = eventData.Reading.Acceleration.Y;
                var z = eventData.Reading.Acceleration.Z;

                _AccelMagnitude = Math.Sqrt(x * x + y * y + z * z);

                Console.WriteLine($"Acceleration: {x}, {y}, {z}");
                Console.WriteLine($"Magnitude: {_AccelMagnitude}");
            };

            Accelerometer.Start(SensorSpeed.UI);

            await Task.Delay(Timeout.Infinite, cancellationToken); // Keep the method running until cancelled
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Acceleration collection cancelled");
        }
    }
}