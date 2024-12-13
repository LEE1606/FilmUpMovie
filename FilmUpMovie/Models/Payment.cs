using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmUpMovie.Models
{
    public class Payment
    {
        [Key]
        [Display(Name = "Payment Slip ID")]
        public int ID { get; set; }

        [Display(Name = "TotalWorkTime (hours)")]
        [Required]
        public double TotalWorkTime { get; set; }

        [DataType(DataType.Date)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PayDate { get; set; }

        [Display(Name = "PayAmount (RM)")]
        public double PayAmount { get; set; }

        // Foreign Key to Staff
        [Required]
        public int StaffID { get; set; }

        [ForeignKey("StaffID")]
        public Staff Staff { get; set; } = null!;
    }
}
