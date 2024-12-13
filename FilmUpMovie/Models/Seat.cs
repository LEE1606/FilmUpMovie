using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmUpMovie.Models
{
    public class Seat
    {
        [Key]
        public int SeatID { get; set; }

        [Required]
        public int ShowTimeID { get; set; }

        [ForeignKey("ShowTimeID")]
        public ShowTime ShowTime { get; set; }

        [Required]
        [Display(Name = "Seat Number")]
        public string SeatNumber { get; set; } = string.Empty;

        // Change SeatStatus to string
        [Required]
        public string Status { get; set; } = "Available"; // Default value

        // Add the Price property
        [Required]
        [Range(0.00, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        // Add an IsAvailable property
        [Required]
        public bool IsAvailable { get; set; }

        [NotMapped]
        public string MovieName => ShowTime?.Movie?.Title ?? "N/A";

        [NotMapped]
        public string ShowtimeDetails => ShowTime != null ? $"{ShowTime.Time}" : "N/A";
    }
}
