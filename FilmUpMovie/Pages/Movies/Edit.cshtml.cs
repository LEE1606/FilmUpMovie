using FilmUpMovie.Authorization;
using FilmUpMovie.Data;
using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilmUpMovie.Pages.Movies
{
    public class EditModel : DI_BasePageModel
    {
        private readonly FilmUpMovie.Data.FilmUpMovieContext _context;
        private readonly IConfiguration _configuration;

        public EditModel(FilmUpMovieContext context, IConfiguration configuration,
        IAuthorizationService authorizationService,
        UserManager<IdentityUser> userManager) : base(context, authorizationService, userManager)
        {
            _context = context;
            _configuration = configuration;
        }

        [BindProperty]
        public Movie Movie { get; set; } = default!;

        public IFormFile? PosterImageFile { get; set; }

        // GET method to fetch the movie data for editing
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FirstOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            Movie = movie;
            return Page();
        }

        // POST method to handle the movie update
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Fetch the movie from DB to get the original data (OwnerID, etc.)
            var movie = await _context.Movie.AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);

            if (movie == null)
            {
                return NotFound();
            }

            // Authorization check to ensure the user has the permission to update the movie
            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, movie, Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            // Set the MovieOwnerId to ensure it doesn't get overwritten
            Movie.MovieOwnerId = movie.MovieOwnerId;

            // If the poster image file is updated, handle the file upload and convert to byte array
            if (PosterImageFile != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(PosterImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("PosterImageFile", "Only image files (jpg, jpeg, png) are allowed.");
                    return Page();
                }

                // Check file size (limit to 10MB)
                if (PosterImageFile.Length > 10 * 1024 * 1024)  // 10MB size limit
                {
                    ModelState.AddModelError("PosterImageFile", "The file size exceeds the 10MB limit.");
                    return Page();
                }

                // Convert the uploaded file to a byte array
                using (var memoryStream = new MemoryStream())
                {
                    await PosterImageFile.CopyToAsync(memoryStream);
                    Movie.PosterImage = memoryStream.ToArray(); // Store the byte array in PosterImage
                }
            }

            // Update movie status logic (keep the status unchanged unless authorized to approve)
            if (Movie.Status == MovieStatus.Available)
            {
                var canApprove = await AuthorizationService.AuthorizeAsync(User, Movie, Operations.Approve);

                if (!canApprove.Succeeded)
                {
                    Movie.Status = MovieStatus.Pending; // Set status back to "Submitted" if the user can't approve
                }
            }

            // Mark the entity as modified and update the database
            _context.Attach(Movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(Movie.ID))
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

        // Helper method to check if a movie exists in the database
        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.ID == id);
        }
    }
}
