using System;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public Guid UserID { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MaxLength(15)]
    public string Sex { get; set; }

    [Required]
    public DateTime DoB { get; set; }

    [Required]
    [MaxLength(100)]
    public string Address { get; set; }

    [MaxLength(15)]
    public string? PhoneNumber { get; set; }

    [MaxLength(100)]
    public string? Email { get; set; }
}