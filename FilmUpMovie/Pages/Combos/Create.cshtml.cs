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

namespace FilmUpMovie.Pages.Combos
{
    [Authorize(Roles = "ComboAdministrators, ComboManager")]
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
        public ComboViewModel ComboViewModel { get; set; } = new ComboViewModel();

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

            // Save the uploaded file to a directory and convert it to byte[]
            byte[] imageBytes = null;
            if (ComboViewModel.ComboImageFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await ComboViewModel.ComboImageFile.CopyToAsync(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }
            }

            // Map the ComboViewModel to the Combo model
            var combo = new Combo
            {
                Name = ComboViewModel.Name,
                Price = ComboViewModel.Price,
                Description = ComboViewModel.Description,
                IsOnSale = ComboViewModel.IsOnSale,
                ComboImage = imageBytes  // Save the image as a byte array
            };

            // Authorization check
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, combo, Operations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();  // Return Forbidden if the user is not authorized to create combos
            }

            _context.Combos.Add(combo);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");  // Redirect to the list of combos after creation
        }
    }
}

