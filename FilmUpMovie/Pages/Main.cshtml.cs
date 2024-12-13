using FilmUpMovie.Data;
using FilmUpMovie.Data;
using FilmUpMovie.Models;  // Import your models
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FilmUpMovie.Pages
{
    [AllowAnonymous]
    public class MainModel : PageModel
    {
        private readonly FilmUpMovieContext _context;

        public MainModel(FilmUpMovieContext context)
        {
            _context = context;
        }

        public IList<Food> Foods { get; set; }
        public IList<Beverage> Beverages { get; set; }
        public IList<Combo> Combos { get; set; }

        // OnGet: Fetch products from the database
        public void OnGet()
        {
            Foods = _context.Foods.ToList();
            Beverages = _context.Beverages.ToList();
            Combos = _context.Combos.ToList();
        }

        // OnPostAddToCart: Handles adding the selected item to the cart
        public IActionResult OnPostAddToCart(int? foodId, int? beverageId, int? comboId)
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

            CartItem cartItem = null;

            // Handle adding Food item to the cart
            if (foodId.HasValue)
            {
                var foodItem = _context.Foods.FirstOrDefault(f => f.F_ID == foodId);
                if (foodItem == null) return NotFound();
                cartItem = new CartItem { CartId = cartId, FoodId = foodItem.F_ID, FoodItem = foodItem, Quantity = 1 };
            }
            // Handle adding Beverage item to the cart
            else if (beverageId.HasValue)
            {
                var beverageItem = _context.Beverages.FirstOrDefault(b => b.B_ID == beverageId);
                if (beverageItem == null) return NotFound();
                cartItem = new CartItem { CartId = cartId, BeverageId = beverageItem.B_ID, BeverageItem = beverageItem, Quantity = 1 };
            }
            // Handle adding Combo item to the cart
            else if (comboId.HasValue)
            {
                var comboItem = _context.Combos.FirstOrDefault(c => c.Combo_ID == comboId);
                if (comboItem == null) return NotFound();
                cartItem = new CartItem { CartId = cartId, ComboId = comboItem.Combo_ID, ComboItem = comboItem, Quantity = 1 };
            }

            // Now, check if the item already exists in the cart by comparing product type and ID
            var existingCartItem = _context.CartItems
                                   .FirstOrDefault(ci => ci.CartId == cartId &&
                                                         ((ci.FoodId == cartItem.FoodId && cartItem.FoodId.HasValue) ||
                                                          (ci.BeverageId == cartItem.BeverageId && cartItem.BeverageId.HasValue) ||
                                                          (ci.ComboId == cartItem.ComboId && cartItem.ComboId.HasValue)));

            if (existingCartItem != null)
            {
                // If the item exists, increase the quantity
                existingCartItem.Quantity += 1;
                _context.CartItems.Update(existingCartItem);
            }
            else
            {
                // If the item doesn't exist, add it to the cart
                _context.CartItems.Add(cartItem);
            }

            _context.SaveChanges(); // Save changes to the database

            return RedirectToPage(); // Refresh the page to show the updated cart
        }
    }
}




