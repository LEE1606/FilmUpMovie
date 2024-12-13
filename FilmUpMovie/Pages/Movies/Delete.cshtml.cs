using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FilmUpMovie.Data;
using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FilmUpMovie.Authorization;

namespace FilmUpMovie.Pages.Movies
{
    public class DeleteModel : DI_BasePageModel
    {
        private readonly FilmUpMovie.Data.FilmUpMovieContext _context;

        public DeleteModel(FilmUpMovieContext context, IConfiguration configuration,
        IAuthorizationService authorizationService,
        UserManager<IdentityUser> userManager) : base(context, authorizationService, userManager)
        {
            _context = context;
        }

        [BindProperty]
        public Movie Movies { get; set; }

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
            else
            {
                Movies = movie;
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                 User, Movies,
                                                 Operations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }
            var movie = await _context.Movie.FindAsync(id);

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                 User, movie,
                                                 Operations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            if (movie != null)
            {
                Movies = movie;
                _context.Movie.Remove(Movies);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
