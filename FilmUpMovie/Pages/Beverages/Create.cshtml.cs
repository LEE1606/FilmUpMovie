using System.IO;
using System.Threading.Tasks;
using FilmUpMovie.Authorization;
using FilmUpMovie.Data;
using FilmUpMovie.Models;
using FilmUpMovie.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmUpMovie.Pages.Beverages
{
    
    public class CreateModel : PageModel
    {
        private readonly FilmUpMovieContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAuthorizationService _authorizationService;

        public CreateModel(
            FilmUpMovieContext context,
            IWebHostEnvironment webHostEnvironment,
            IAuthorizationService authorizationService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public BeverageViewModel BeverageViewModel { get; set; } = new BeverageViewModel();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Map the BeverageViewModel to the Beverage model
            var beverage = new Beverage
            {
                Name = BeverageViewModel.Name,
                Price = BeverageViewModel.Price,
                Description = BeverageViewModel.Description,
             
                IsNewArrival = BeverageViewModel.IsNewArrival,
                IsOnSale = BeverageViewModel.IsOnSale
            };

            // Authorization check for creating beverages
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, beverage, Operations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid(); // Return Forbidden if the user is not authorized
            }

            // Save the uploaded file to a directory and convert it to byte[] (for image)
            if (BeverageViewModel.ImageFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await BeverageViewModel.ImageFile.CopyToAsync(memoryStream);
                    beverage.BeverageImage = memoryStream.ToArray();
                }
            }

            _context.Beverages.Add(beverage);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}