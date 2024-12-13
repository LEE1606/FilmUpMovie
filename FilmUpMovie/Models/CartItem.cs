using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmUpMovie.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; } // Unique identifier for CartItem

        [Required]
        public int CartId { get; set; } // Foreign key to the Cart

        // Foreign key to Food, Beverage, or Combo
        public int? FoodId { get; set; }
        public int? BeverageId { get; set; }
        public int? ComboId { get; set; }

        // Navigation properties
        public Food FoodItem { get; set; }
        public Beverage BeverageItem { get; set; }
        public Combo ComboItem { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; } // Quantity of the product in the cart
    }
}

