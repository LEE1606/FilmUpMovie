using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FilmUpMovie.Data;
using FilmUpMovie.Models;
using FilmUpMovie.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FilmUpMovie.Pages.Movies
{
    public class DetailsModel : DI_BasePageModel
    {
        private readonly FilmUpMovie.Data.FilmUpMovieContext _context;

        public DetailsModel(FilmUpMovieContext context, IConfiguration configuration,
        IAuthorizationService authorizationService,
        UserManager<IdentityUser> userManager) : base(context, authorizationService, userManager)
        {
            _context = context;
        }

        public Movie Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            // Include related ShowTimes in the query
            var movie = await _context.Movie
                .Include(m => m.ShowTimes) // Include related ShowTimes
                .FirstOrDefaultAsync(m => m.ID == id);

            if (movie == null)
            {
                return NotFound();
            }
            else
            {
                Movie = movie;
            }

            var isAuthorized = User.IsInRole(Constants.MovieManagersRole) ||
                               User.IsInRole(Constants.MovieAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            if (!isAuthorized
                && currentUserId != Movie.MovieOwnerId
                && Movie.Status != MovieStatus.Available)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, MovieStatus status)
        {
            var movie = await Context.Movie.FirstOrDefaultAsync(
                                                      m => m.ID == id);

            if (movie == null)
            {
                return NotFound();
            }

            var movieOperation = (status == MovieStatus.Available)
                                                       ? Operations.Approve
                                                       : Operations.Reject;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, movie,
                                        movieOperation);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            movie.Status = status;
            Context.Movie.Update(movie);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
