using FilmUpMovie.Authorization;
using FilmUpMovie.Data;
using FilmUpMovie.Models;
using FilmUpMovie.Pages.Movies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace FilmUpMovie.Pages.Movies
{
    [Authorize(Roles = "MovieAdministrator")]
    public class CreateModel : DI_BasePageModel
    {
        private readonly FilmUpMovie.Data.FilmUpMovieContext _context;

        public CreateModel(FilmUpMovie.Data.FilmUpMovieContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager) : base(context, authorizationService, userManager)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        // Movie model bound to the page
        [BindProperty]
        public Movie Movie { get; set; }

        public IFormFile PosterImageFile { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validate the model state
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Assign the MovieOwnerId from the logged-in user
            Movie.MovieOwnerId = UserManager.GetUserId(User);

            // Authorization check: ensure user is authorized to create a movie
            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, Movie, Operations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();  // If not authorized, return a "Forbidden" response
            }

            // Handle the image file upload if a file is provided
            if (PosterImageFile != null)
            {
                // Validate file type (e.g., jpg, jpeg, png)
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(PosterImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("PosterImageFile", "Only image files (jpg, jpeg, png) are allowed.");
                    return Page();  // Return with error if file is not valid
                }

                // Check the file size (e.g., limit to 10MB)
                if (PosterImageFile.Length > 10 * 1024 * 1024)  // 10MB limit
                {
                    ModelState.AddModelError("PosterImageFile", "The file size exceeds the 10MB limit.");
                    return Page();  // Return with error if the file size is too large
                }

                // Convert the image file to a byte array
                using (var memoryStream = new MemoryStream())
                {
                    await PosterImageFile.CopyToAsync(memoryStream);
                    Movie.PosterImage = memoryStream.ToArray();  // Save the byte array to the Movie model
                }
            }

            // Add the movie to the database
            _context.Movie.Add(Movie);
            await _context.SaveChangesAsync();

            // Redirect to the Index page after successful movie creation
            return RedirectToPage("./Index");
        }
    }
}
