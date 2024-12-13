using System.ComponentModel.DataAnnotations;

namespace FilmUpMovie.Models
{
    public class Staff
    {
        public int ID { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "The name is over 20 character!")]
        [Display(Name = "Name")]
        public string StaffName { get; set; } = string.Empty;
        [Required]
        [Range(18, 70, ErrorMessage = "Age can only between 18 - 70")]
        [Display(Name = "Age")]
        public int StaffAge { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "The email is over 20 character!")]
        [Display(Name = "Email")]
        public string StaffEmail { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Phone Number")]
        public int StaffPhoneNumber { get; set; }
        [Display(Name = "Home Address")]
        public string StaffAddress { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",
                       ApplyFormatInEditMode = true)]
        [Display(Name = "Started Date")]
        public DateTime StaffWorkDate { get; set; }
    }
}
