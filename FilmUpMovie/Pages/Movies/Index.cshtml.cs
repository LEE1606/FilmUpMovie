using FilmUpMovie.Authorization;
using FilmUpMovie.Data;
using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FilmUpMovie.Pages.Movies
{
    [AllowAnonymous]
    public class IndexModel : DI_BasePageModel
    {
        private readonly FilmUpMovieContext _context;
        private readonly IConfiguration Configuration;

        public IndexModel(FilmUpMovieContext context, IConfiguration configuration,
        IAuthorizationService authorizationService,
        UserManager<IdentityUser> userManager) : base(context, authorizationService, userManager)
        {
            _context = context;
            Configuration = configuration;
        }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public string MovieStatusFilter { get; set; }  // Movie status filter (e.g., "NowShowing", "ComingSoon")

        public List<Movie> Movies { get; set; }

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, string movieStatusFilter, int? pageIndex)
        {
            // Handle the sorting options
            CurrentSort = sortOrder;
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            // Search functionality
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;
            MovieStatusFilter = movieStatusFilter ?? "All";  // Store the selected filter for use in the query

            // Start querying the Movie table
            IQueryable<Movie> MoviesIQ = from s in _context.Movie
                                         select s;

            // Apply the search filter if searchString is provided
            if (!String.IsNullOrEmpty(searchString))
            {
                MoviesIQ = MoviesIQ.Where(s => s.Title.Contains(searchString));
            }

            // Apply status filter based on the MovieStatusFilter parameter
            if (!string.IsNullOrEmpty(movieStatusFilter))
            {
                if (movieStatusFilter == "NowShowing")
                {
                    // Show movies that are approved and have a release date in the past (Now Showing)
                    MoviesIQ = MoviesIQ.Where(m => m.Status == MovieStatus.Available && m.ReleaseDate <= DateTime.Now);
                }
                else if (movieStatusFilter == "ComingSoon")
                {
                    // Show movies with a future release date (Coming Soon)
                    MoviesIQ = MoviesIQ.Where(m => m.ReleaseDate > DateTime.Now);
                }
            }

            // Sorting based on the sortOrder value
            switch (sortOrder)
            {
                case "name_desc":
                    MoviesIQ = MoviesIQ.OrderByDescending(s => s.Title);
                    break;
                case "Date":
                    MoviesIQ = MoviesIQ.OrderBy(s => s.ReleaseDate);
                    break;
                case "date_desc":
                    MoviesIQ = MoviesIQ.OrderByDescending(s => s.ReleaseDate);
                    break;
                default:
                    MoviesIQ = MoviesIQ.OrderBy(s => s.Title);
                    break;
            }

            // Set the page size and get the paginated list
            var pageSize = Configuration.GetValue("PageSize", 4);

            // Authorization check: Determine if the user is an admin or manager
            var isAuthorized = User.IsInRole(Constants.MovieManagersRole) || User.IsInRole(Constants.MovieAdministratorsRole);
            var currentUserId = UserManager.GetUserId(User);

            // Show only approved movies or movies owned by the current user if the user is not authorized
            if (!isAuthorized)
            {
                MoviesIQ = MoviesIQ.Where(c => c.Status == MovieStatus.Available || c.MovieOwnerId == currentUserId);
            }

            // Get the filtered list of movies
            Movies = await MoviesIQ.ToListAsync();
        }
    }
}
