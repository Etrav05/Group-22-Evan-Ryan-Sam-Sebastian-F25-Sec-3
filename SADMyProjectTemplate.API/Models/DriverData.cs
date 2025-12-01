namespace SADMyProjectTemplate.API.Models
{
    public class DriverData
    {
        public int DriverId { get; set; }  // Primary key
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Speed { get; set; }
        public double Acceleration { get; set; }
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;
    }
}
