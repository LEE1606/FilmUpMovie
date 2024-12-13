using FilmUpMovie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using FilmUpMovie.Data;

namespace FilmUpMovie.Pages
{
    public class ComboMainModel : PageModel
    {
        private readonly FilmUpMovieContext _context;

        public ComboMainModel(FilmUpMovieContext context)
        {
            _context = context;
        }

        public IList<Combo> Combos { get; set; }

        // OnGet: Fetch combo items from the database
        public void OnGet()
        {
            Combos = _context.Combos.ToList();
        }

        // OnPostAddToCart: Handles adding the selected combo item to the cart
        public IActionResult OnPostAddToCart(int comboId)
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

            var comboItem = _context.Combos.FirstOrDefault(c => c.Combo_ID == comboId);
            if (comboItem == null) return NotFound();

            // Check if the item already exists in the cart
            var cartItem = _context.CartItems
                                   .FirstOrDefault(ci => ci.CartId == cartId && ci.ComboId == comboItem.Combo_ID);

            if (cartItem != null)
            {
                // If the item exists, increase the quantity
                cartItem.Quantity += 1;
                _context.CartItems.Update(cartItem);
            }
            else
            {
                // If the item doesn't exist, add it to the cart
                cartItem = new CartItem { CartId = cartId, ComboId = comboItem.Combo_ID, ComboItem = comboItem, Quantity = 1 };
                _context.CartItems.Add(cartItem);
            }

            _context.SaveChanges(); // Save changes to the database
            return RedirectToPage(); // Refresh the page
        }
    }
}
