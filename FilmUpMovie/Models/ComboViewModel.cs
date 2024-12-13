using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FilmUpMovie.ViewModels
{
    public class ComboViewModel
    {
        public int Combo_ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public bool IsOnSale { get; set; }

        [Display(Name = "Combo Image")]
        public IFormFile ComboImageFile { get; set; } // Renamed for consistency
    }
}
