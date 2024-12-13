using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FilmUpMovie.Authorization;
using FilmUpMovie.Data;
using FilmUpMovie.Models;
using FilmUpMovie.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FilmUpMovie.Pages.Foods
{
    [Authorize(Roles = "FoodAdministrators")]
    public class EditModel : PageModel
    {
        private readonly FilmUpMovieContext _context;
        private readonly IAuthorizationService _authorizationService;

        public EditModel(FilmUpMovieContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public FoodViewModel FoodViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            // Authorization check before editing
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, food, Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            // Map entity data to the view model
            FoodViewModel = new FoodViewModel
            {
                Name = food.Name,
                Price = food.Price,
                Description = food.Description,
                Category = food.Category,
                IsNewArrival = food.IsNewArrival,
                IsOnSale = food.IsOnSale
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var food = await _context.Foods.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            // Authorization check before updating
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, food, Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            // Update fields
            food.Name = FoodViewModel.Name;
            food.Price = FoodViewModel.Price;
            food.Description = FoodViewModel.Description;
            food.Category = FoodViewModel.Category;
            food.IsNewArrival = FoodViewModel.IsNewArrival;
            food.IsOnSale = FoodViewModel.IsOnSale;

            // Handle optional file upload
            if (FoodViewModel.ImageFile != null)
            {
                // Validate the file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(FoodViewModel.ImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("FoodViewModel.ImageFile", "Only .jpg, .jpeg, or .png files are allowed.");
                    return Page();
                }

                // Convert image to byte array
                using (var memoryStream = new MemoryStream())
                {
                    await FoodViewModel.ImageFile.CopyToAsync(memoryStream);
                    food.FoodImage = memoryStream.ToArray();
                }
            }

            try
            {
                _context.Foods.Update(food);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Foods.Any(e => e.F_ID == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
    }
}


