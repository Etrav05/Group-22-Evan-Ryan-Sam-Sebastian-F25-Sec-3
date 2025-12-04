using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Trips
{
    [Key]
    public Guid TripID { get; set; }

    [Required]
    public Guid AccountID { get; set; }

    [ForeignKey(nameof(AccountID))]
    public Account Account { get; set; }

    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    public double? StartLatitude { get; set; }
    public double? StartLongitude { get; set; }
    public double? EndLatitude { get; set; }
    public double? EndLongitude { get; set; }

    public double? TotalDistance { get; set; }
    public double? AvgSpeed { get; set; }
    public double? MaxSpeed { get; set; }

    public int? FaultData { get; set; }
}
