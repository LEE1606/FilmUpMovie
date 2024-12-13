using FilmUpMovie.Data;
using FilmUpMovie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace FilmUpMovie.Pages
{
    public class FoodMainModel : PageModel
    {
        private readonly FilmUpMovieContext _context;

        public FoodMainModel(FilmUpMovieContext context)
        {
            _context = context;
        }

        public IList<Food> Foods { get; set; }

        // OnGet: Fetch food items from the database
        public void OnGet()
        {
            Foods = _context.Foods.ToList();
        }

        // OnPostAddToCart: Handles adding the selected food item to the cart
        public IActionResult OnPostAddToCart(int foodId)
        {
            var cartId = HttpContext.Session.GetInt32("CartId") ?? 0;

            // If no CartId exists, create a new cart
            if (cartId == 0)
            {
                var newCart = new Cart { Items = new List<CartItem>() };
                _context.Carts.Add(newCart);
                _context.SaveChanges();
                cartId = newCart.CartId;
                HttpContext.Session.SetInt32("CartId", cartId); // Store CartId in the session
            }

            var foodItem = _context.Foods.FirstOrDefault(f => f.F_ID == foodId);
            if (foodItem == null) return NotFound();

            // Check if the item already exists in the cart
            var cartItem = _context.CartItems
                                   .FirstOrDefault(ci => ci.CartId == cartId && ci.FoodId == foodItem.F_ID);

            if (cartItem != null)
            {
                // If the item exists, increase the quantity
                cartItem.Quantity += 1;
                _context.CartItems.Update(cartItem);
            }
            else
            {
                // If the item doesn't exist, add it to the cart
                cartItem = new CartItem { CartId = cartId, FoodId = foodItem.F_ID, FoodItem = foodItem, Quantity = 1 };
                _context.CartItems.Add(cartItem);
            }

            _context.SaveChanges(); // Save changes to the database
            return RedirectToPage(); // Refresh the page
        }
    }
}
