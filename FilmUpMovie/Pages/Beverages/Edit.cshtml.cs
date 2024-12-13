using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FilmUpMovie.Data;
using FilmUpMovie.Models;
using FilmUpMovie.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using FilmUpMovie.Authorization;

namespace FilmUpMovie.Pages.Beverages
{
    [Authorize(Roles = Constants.BeverageAdministratorsRole)]
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
        public BeverageViewModel BeverageViewModel { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var beverage = await _context.Beverages.FindAsync(id);
            if (beverage == null)
            {
                return NotFound();
            }

            // Authorization check before allowing editing
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, beverage, Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid(); // Return Forbidden if not authorized
            }

            // Map Beverage entity to BeverageViewModel
            BeverageViewModel = new BeverageViewModel
            {
                Name = beverage.Name,
                Price = beverage.Price,
                Description = beverage.Description,
                IsNewArrival = beverage.IsNewArrival,
                IsOnSale = beverage.IsOnSale
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var beverage = await _context.Beverages.FindAsync(id);
            if (beverage == null)
            {
                return NotFound();
            }

            // Authorization check before updating
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, beverage, Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid(); // Return Forbidden if not authorized
            }

            // Update Beverage entity with BeverageViewModel values
            beverage.Name = BeverageViewModel.Name;
            beverage.Price = BeverageViewModel.Price;
            beverage.Description = BeverageViewModel.Description;
            beverage.IsNewArrival = BeverageViewModel.IsNewArrival;
            beverage.IsOnSale = BeverageViewModel.IsOnSale;

            // Handle file upload if a new image is provided
            if (BeverageViewModel.ImageFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await BeverageViewModel.ImageFile.CopyToAsync(memoryStream);
                    beverage.BeverageImage = memoryStream.ToArray();
                }
            }

            try
            {
                _context.Beverages.Update(beverage);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Beverages.Any(e => e.B_ID == id))
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

