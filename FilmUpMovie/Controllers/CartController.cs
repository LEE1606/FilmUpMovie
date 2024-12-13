using FilmUpMovie.Models;
using FilmUpMovie.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace FilmUpMovie.Controllers
{
    public class CartController : Controller
    {
        private readonly FilmUpMovieContext _db;

        public CartController(FilmUpMovieContext db)
        {
            _db = db;
        }

        // View the Cart - Display Cart and Items
        public IActionResult Index()
        {
            var cartId = HttpContext.Session.GetInt32("CartId") ?? 0;

            // If cartId doesn't exist, create a new cart
            if (cartId == 0)
            {
                var newCart = new Cart { Items = new List<CartItem>() };
                _db.Carts.Add(newCart);
                _db.SaveChanges();
                cartId = newCart.CartId;
                HttpContext.Session.SetInt32("CartId", cartId);
            }

            // Retrieve the cart from the database
            var cart = _db.Carts
                .Include(c => c.Items)
                    .ThenInclude(ci => ci.FoodItem)
                .Include(c => c.Items)
                    .ThenInclude(ci => ci.BeverageItem)
                .Include(c => c.Items)
                    .ThenInclude(ci => ci.ComboItem)
                .FirstOrDefault(c => c.CartId == cartId);

            // If no cart is found, initialize it as a new cart
            if (cart == null)
            {
                cart = new Cart { Items = new List<CartItem>() };
            }

            // Ensure Items is not null
            if (cart.Items == null)
            {
                cart.Items = new List<CartItem>();
            }

            return View(cart); // Return the cart to the view
        }

        // OnPostAddToCart: Adds an item to the cart (called from POST request when user clicks "Add to Cart")
        [HttpPost]
        public IActionResult OnPostAddToCart(int? foodId, int? beverageId, int? comboId)
        {
            var cartId = HttpContext.Session.GetInt32("CartId") ?? 0;

            if (cartId == 0)
            {
                var newCart = new Cart { Items = new List<CartItem>() };
                _db.Carts.Add(newCart);
                _db.SaveChanges();
                cartId = newCart.CartId;
                HttpContext.Session.SetInt32("CartId", cartId);
            }

            CartItem cartItem = null;

            if (foodId.HasValue)
            {
                var foodItem = _db.Foods.FirstOrDefault(f => f.F_ID == foodId);
                if (foodItem == null) return NotFound();
                cartItem = new CartItem { CartId = cartId, FoodId = foodItem.F_ID, FoodItem = foodItem, Quantity = 1 };
            }
            else if (beverageId.HasValue)
            {
                var beverageItem = _db.Beverages.FirstOrDefault(b => b.B_ID == beverageId);
                if (beverageItem == null) return NotFound();
                cartItem = new CartItem { CartId = cartId, BeverageId = beverageItem.B_ID, BeverageItem = beverageItem, Quantity = 1 };
            }
            else if (comboId.HasValue)
            {
                var comboItem = _db.Combos.FirstOrDefault(c => c.Combo_ID == comboId);
                if (comboItem == null) return NotFound();
                cartItem = new CartItem { CartId = cartId, ComboId = comboItem.Combo_ID, ComboItem = comboItem, Quantity = 1 };
            }

            if (cartItem != null)
            {
                // Check if the item already exists in the cart
                var existingCartItem = _db.CartItems
                    .FirstOrDefault(ci => ci.CartId == cartId &&
                                          (ci.FoodId == cartItem.FoodId ||
                                           ci.BeverageId == cartItem.BeverageId ||
                                           ci.ComboId == cartItem.ComboId));

                if (existingCartItem != null)
                {
                    // Update quantity if the item already exists in the cart
                    existingCartItem.Quantity += 1;
                    _db.CartItems.Update(existingCartItem);
                }
                else
                {
                    // Add new item to the cart
                    _db.CartItems.Add(cartItem);
                }

                _db.SaveChanges();
            }

            return RedirectToAction("Index"); // Refresh the page
        }

        // OnPostUpdateQuantity: Updates the quantity of an item in the cart
        [HttpPost]
        public IActionResult OnPostUpdateQuantity(int cartItemId, int quantity)
        {
            if (quantity < 1)
            {
                // If quantity is less than 1, remove the item from the cart
                var cartItem = _db.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);
                if (cartItem == null)
                {
                    return NotFound();
                }

                _db.CartItems.Remove(cartItem);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            var cartItemToUpdate = _db.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);
            if (cartItemToUpdate == null)
            {
                return NotFound();
            }

            cartItemToUpdate.Quantity = quantity;
            _db.CartItems.Update(cartItemToUpdate);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // OnPostRemoveItem: Removes an item from the cart
        [HttpPost]
        public IActionResult OnPostRemoveItem(int cartItemId)
        {
            var cartItem = _db.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);
            if (cartItem == null)
            {
                return NotFound();
            }

            _db.CartItems.Remove(cartItem);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
