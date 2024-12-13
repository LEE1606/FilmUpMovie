using FilmUpMovie.Authorization;
using FilmUpMovie.Data;
using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FilmUpMovie.Pages.Cinemas
{
    [Authorize(Roles = "CinemaAdministrator")]
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

        [BindProperty]
        public Cinema Cinema { get; set; }
        public IFormFile CinemaImageFile { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Cinema.CinemaOwnerid = UserManager.GetUserId(User);

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                        User, Cinema,
                                                        Operations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            // Handle the image file upload if a file is provided
            if (CinemaImageFile != null)
            {
                // Validate file type (e.g., jpg, jpeg, png)
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(CinemaImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("PosterImageFile", "Only image files (jpg, jpeg, png) are allowed.");
                    return Page();  // Return with error if file is not valid
                }

                // Check the file size (e.g., limit to 10MB)
                if (CinemaImageFile.Length > 10 * 1024 * 1024)  // 10MB limit
                {
                    ModelState.AddModelError("PosterImageFile", "The file size exceeds the 10MB limit.");
                    return Page();  // Return with error if the file size is too large
                }

                // Convert the image file to a byte array
                using (var memoryStream = new MemoryStream())
                {
                    await CinemaImageFile.CopyToAsync(memoryStream);
                    Cinema.CinemaImage = memoryStream.ToArray();  // Save the byte array to the Movie model
                }
            }

            _context.Cinema.Add(Cinema);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}