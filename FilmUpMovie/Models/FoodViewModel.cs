using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FilmUpMovie.ViewModels
{
    public class FoodViewModel
    {
        public int F_ID { get; set; } // Add this property to match the database primary key

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public bool IsNewArrival { get; set; }

        public bool IsOnSale { get; set; }

        [Display(Name = "Upload Image")]
        public IFormFile ImageFile { get; set; }
    }
}



