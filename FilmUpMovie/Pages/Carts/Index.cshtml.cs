using System.Net.NetworkInformation;
using FilmUpMovie.Models;
using FilmUpMovie.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FlimUPMovie.Pages.Carts
{
    public class IndexModel : PageModel
    {
        private readonly FilmUpMovieContext _db;

        public IndexModel(FilmUpMovieContext db)
        {
            _db = db;
        }

        public Cart Cart { get; set; }

        // OnGet: Retrieves the cart and its items (called when the page is loaded)
        public void OnGet()
        {
            var cartId = HttpContext.Session.GetInt32("CartId") ?? 0;

            // If no cart ID exists in the session, create a new cart
            if (cartId == 0)
            {
                Cart = new Cart { Items = new List<CartItem>() };  // Initialize with an empty list of items
            }
            else
            {
                // Retrieve the cart with its items from the database
                Cart = _db.Carts
                          .Include(c => c.Items)
                          .ThenInclude(ci => ci.FoodItem)
                          .Include(c => c.Items)
                          .ThenInclude(ci => ci.BeverageItem)
                          .Include(c => c.Items)
                          .ThenInclude(ci => ci.ComboItem)
                          .FirstOrDefault(c => c.CartId == cartId);

                // Ensure Cart is not null
                if (Cart == null)
                {
                    Cart = new Cart { Items = new List<CartItem>() };  // Initialize with an empty list if not found
                }
            }
        }

        // OnPostUpdateQuantity: Updates the quantity of an item in the cart
        public IActionResult OnPostUpdateQuantity(int cartItemId, int quantity)
        {
            if (quantity < 1)
            {
                return BadRequest("Quantity must be at least 1.");
            }

            // Retrieve the cart item
            var cartItem = _db.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);
            if (cartItem == null)
            {
                return NotFound();  // Return 404 if the item is not found
            }

            // Update the quantity of the item
            cartItem.Quantity = quantity;
            _db.CartItems.Update(cartItem);
            _db.SaveChanges();

            // Redirect to refresh the page
            return RedirectToPage();
        }

        // OnPostRemoveItem: Removes an item from the cart
        public IActionResult OnPostRemoveItem(int cartItemId)
        {
            // Retrieve the cart item to remove
            var cartItem = _db.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);
            if (cartItem == null)
            {
                return NotFound();  // Return 404 if the item is not found
            }

            // Remove the cart item from the database
            _db.CartItems.Remove(cartItem);
            _db.SaveChanges();

            // Redirect to refresh the page
            return RedirectToPage();
        }
    }
}

