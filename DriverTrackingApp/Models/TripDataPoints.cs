using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TripDataPoints
{
    [Key]
    public int DataPointID { get; set; }  // AUTO_INCREMENT matches int identity

    [Required]
    public Guid TripID { get; set; }

    [ForeignKey(nameof(TripID))]
    public Trips Trip { get; set; }

    [Required]
    public Guid AccountID { get; set; }

    [ForeignKey(nameof(AccountID))]
    public Account Account { get; set; }

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public double? Speed { get; set; }
    public double? Acceleration { get; set; }

    [Required]
    public DateTime TimeData { get; set; } = DateTime.UtcNow;
}
