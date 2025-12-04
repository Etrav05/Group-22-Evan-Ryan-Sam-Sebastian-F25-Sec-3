using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DriverTrackingApp.Services
{
    public class ProcessTrip
    {
        private readonly AppDbContext _db;

        public ProcessTrip(AppDbContext db)
        {
            _db = db;
        }

        public async Task ProcessTripSummary(Guid tripId)
        {
            // Get all data points for the trip
            var dataPoints = await _db.TripDataPoints
                .Where(p => p.TripID == tripId)
                .OrderBy(p => p.TimeData)
                .ToListAsync();

            if (dataPoints.Count == 0) // If theres nothing to process then exit
            {
                Console.WriteLine("No data points to process");
                return;
            }

            var trip = await _db.TripData.FindAsync(tripId); // Get the trip record

            if (trip == null)
            {
                Console.WriteLine("Trip not found!");
                return;
            }

            // Calculate summary statistics (easy)
            trip.EndTime = DateTime.UtcNow;
            trip.StartLatitude = dataPoints.First().Latitude;
            trip.StartLongitude = dataPoints.First().Longitude;
            trip.EndLatitude = dataPoints.Last().Latitude;
            trip.EndLongitude = dataPoints.Last().Longitude;

            // Calculate total distance
            trip.TotalDistance = CalculateTotalDistance(dataPoints);

            // Speed statistics
            trip.AvgSpeed = dataPoints.Average(p => p.Speed);
            trip.MaxSpeed = dataPoints.Max(p => p.Speed);

            // Calculate faults: TODO
            // trip.FaultData = CalculateFaults(dataPoints);

            await _db.SaveChangesAsync();

            Console.WriteLine($"Trip summary processed: Distance={trip.TotalDistance:F2}km, AvgSpeed={trip.AvgSpeed:F2}, Faults=N/A"); // TODO: Faults
        }

        private double CalculateTotalDistance(List<TripDataPoints> points)
        {
            double totalDistance = 0;

            for (int i = 1; i < points.Count; i++)
            {
                // Skip if any coordinates are null (Haversine formula requires doubles not double? which is what we store in our models)
                if (points[i - 1].Latitude == null || points[i - 1].Longitude == null ||
                    points[i].Latitude == null || points[i].Longitude == null)
                {
                    continue;
                }

                // To calcuate total distance, we sum the distances between each consecutive point
                // This is made difficult by the fact we get GPS coordinates (lat/lon) not cartesian (x,y) so we have to use this fancy Haversine formula
                // This fomrula calculates the distance between two points on a sphere given their longitudes and latitudes (Becuase the earth isnt flat unfortunately))
                totalDistance += HaversineDistance(
                    points[i - 1].Latitude.Value, points[i - 1].Longitude.Value,
                    points[i].Latitude.Value, points[i].Longitude.Value
                );
            }

            return totalDistance;
        }

        // Haversine formula to calculate distance between two GPS coordinates (This is from the internet, I am not a genius)
        private double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth's radius in kilometers

            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // Distance in kilometers
        }

        private double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        /*
        private int CalculateFaults(List<TripDataPoints> points)
        {

            return 0;
        }
        */
    }
}
