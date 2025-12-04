using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DriverTrackingApp.Services
{
    public class GetTrips
    {
        private readonly AppDbContext _db;

        public GetTrips(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Trips>> GetAllTrips()
        {
            return await _db.TripData
                .OrderByDescending(t => t.StartTime)
                .ToListAsync();
        }

        // Get a trip and its raw data points separately
        public async Task<(Trips trip, List<TripDataPoints> dataPoints)> GetTripWithDataPoints(Guid tripId)
        {
            var trip = await _db.TripData
                .FirstOrDefaultAsync(t => t.TripID == tripId);

            var dataPoints = await _db.TripDataPoints
                .Where(p => p.TripID == tripId)
                .OrderBy(p => p.TimeData)
                .ToListAsync();

            return (trip, dataPoints);
        }

        public async Task<List<TripDataPoints>> GetTripDataPoints(Guid tripId)
        {
            return await _db.TripDataPoints
                .Where(p => p.TripID == tripId)
                .OrderBy(p => p.TimeData)
                .ToListAsync();
        }
    }
}