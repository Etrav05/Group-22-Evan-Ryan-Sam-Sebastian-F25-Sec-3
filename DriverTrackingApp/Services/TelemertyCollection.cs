public class TelemetryService
{
    public async Task StartAsync()
    {
        // Request location permissions first (An android thing)
        var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        if (status != PermissionStatus.Granted)
        {
            Console.WriteLine("Location permission NOT granted");
            return;
        }

        _ = StartLocationCollection();
        StartAccelerationCollection();
    }

    // Speed is also found in the location data
    private async Task StartLocationCollection()
    {
        var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        if (status != PermissionStatus.Granted)
        {
            Console.WriteLine("Location permission was denied.");
            return;
        }

        var request = new GeolocationRequest(GeolocationAccuracy.High);

        while (true)
        {
            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
                Console.WriteLine($"Lat:{location.Latitude} Lon:{location.Longitude} Speed:{location.Speed ?? 0}");

            await Task.Delay(1000);
        }
    }

    private void StartAccelerationCollection()
    {
        Accelerometer.ReadingChanged += (sender, eventData) =>
        {
            Console.WriteLine(
                $"Accel X:{eventData.Reading.Acceleration.X}, " +
                $"Y:{eventData.Reading.Acceleration.Y}, " +
                $"Z:{eventData.Reading.Acceleration.Z}");
        };

        Accelerometer.Start(SensorSpeed.UI);
    }
}