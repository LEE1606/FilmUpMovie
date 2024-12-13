using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmUpMovie.Data; // Ensure this points to your DbContext or data layer
using FilmUpMovie.Models; // Ensure this points to your Movie model
using Microsoft.AspNetCore.Authorization;

namespace FilmUpMovie.Pages
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly FilmUpMovieContext _context;

        public IndexModel(FilmUpMovieContext context)
        {
            _context = context;
        }

        // List of movies to display
        public List<Movie> Movies { get; set; }

        // Current filter (All, NowShowing, ComingSoon)
        public string MovieStatusFilter { get; set; }
        public string CurrentFilter { get; set; }


        public async Task OnGetAsync(string movieStatusFilter, string searchString)
        {
            CurrentFilter = searchString;
            // Set the filter
            MovieStatusFilter = movieStatusFilter ?? "All";

            // Query movies based on the filter
            IQueryable<Movie> movieQuery = from s in _context.Movie
                                           select s;

            // Apply the search filter if searchString is provided
            if (!String.IsNullOrEmpty(searchString))
            {
                movieQuery = movieQuery.Where(s => s.Title.Contains(searchString));
            }

            // Apply status filter based on the MovieStatusFilter parameter
            if (!string.IsNullOrEmpty(movieStatusFilter))
            {
                if (movieStatusFilter == "NowShowing")
                {
                    // Show movies that are approved and have a release date in the past (Now Showing)
                    movieQuery = movieQuery.Where(m => m.Status == MovieStatus.Available && m.ReleaseDate <= DateTime.Now);
                }
                else if (movieStatusFilter == "ComingSoon")
                {
                    // Show movies with a future release date (Coming Soon)
                    movieQuery = movieQuery.Where(m => m.ReleaseDate > DateTime.Now);
                }
            }
            // Get the filtered list of movies
            Movies = await movieQuery.ToListAsync();
        }
    }
}
