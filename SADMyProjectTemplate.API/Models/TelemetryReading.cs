using System;

namespace SADMyProjectTemplate.API.Models
{
    public class TelemetryReading
    {
        public Guid ReadingId { get; set; } = Guid.NewGuid();  // Primary key
        public string TelemetryName { get; set; }
        public double Value { get; set; }
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;
    }
}