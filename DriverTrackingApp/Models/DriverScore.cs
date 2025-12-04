using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class DriverScore
{
    [Key]
    public int ScoreID { get; set; }


    [Required]
    public Guid AccountID { get; set; }

    [ForeignKey(nameof(AccountID))]
    public Account Account { get; set; }

    [Required]
    public int ScoreValue { get; set; }

    [Required]
    public DateTime ScoreDate { get; set; } = DateTime.UtcNow;

}