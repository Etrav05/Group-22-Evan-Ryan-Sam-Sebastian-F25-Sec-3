using System;
using System.ComponentModel.DataAnnotations;

public class Account
{
    [Key]
    public Guid AccountID { get; set; }

    [Required]
    [MaxLength(50)]
    public string password { get; set; }

    [Required]
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}
