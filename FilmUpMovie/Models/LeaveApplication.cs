using FilmUpMovie.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class LeaveApplication
{
    [Key]
    [Display(Name = "Leave Application ID")]
    public int ID { get; set; }

    [Required(ErrorMessage = "Please follow the format as 00:00:00!")]
    public TimeSpan LeaveAppTime { get; set; }

    [DataType(DataType.Date)]
    [Required]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true )]
    public DateTime LeaveAppDate { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string LeaveAppReason { get; set; } = string.Empty;

    // Foreign Key to Staff
    [Required]
    public int StaffID { get; set; }

    [ForeignKey("StaffID")]
    public Staff Staff { get; set; } = null!;

    // User ID from AspNetUser table
    public string? OwnerID { get; set; }

    // Status of the LeaveApplication
    public LeaveApplicationStatus Status { get; set; }
}

public enum LeaveApplicationStatus
{
    Submitted,
    Approved,
    Rejected
}
