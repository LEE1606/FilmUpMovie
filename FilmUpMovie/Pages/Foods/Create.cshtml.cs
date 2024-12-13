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

namespace FilmUpMovie.Pages.Foods
{
    [Authorize(Roles = Constants.FoodAdministratorsRole)]
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
        public FoodViewModel FoodViewModel { get; set; } = new FoodViewModel();

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

            // Map the FoodViewModel to the Food model
            var food = new Food
            {
                Name = FoodViewModel.Name,
                Price = FoodViewModel.Price,
                Description = FoodViewModel.Description,
                Category = FoodViewModel.Category,
                IsNewArrival = FoodViewModel.IsNewArrival,
                IsOnSale = FoodViewModel.IsOnSale,
            };

            // Authorization check for creating food
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, food, Operations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid(); // Return Forbidden if the user is not authorized
            }

            // Save the uploaded file to a directory and convert it to byte[]
            if (FoodViewModel.ImageFile != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await FoodViewModel.ImageFile.CopyToAsync(memoryStream);
                    food.FoodImage = memoryStream.ToArray();
                }
            }

            _context.Foods.Add(food);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
