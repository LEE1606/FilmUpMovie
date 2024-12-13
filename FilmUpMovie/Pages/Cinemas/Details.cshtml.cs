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

namespace FilmUpMovie.Pages.Cinemas
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

        public Cinema Cinema { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Cinema == null)
            {
                return NotFound();
            }

            var cinema = await _context.Cinema.FirstOrDefaultAsync(m => m.CinemaID == id);
            if (cinema == null)
            {
                return NotFound();
            }
            else
            {
                Cinema = cinema;
            }

            var isAuthorized = User.IsInRole(Constants.CinemaManagerRole) ||
                           User.IsInRole(Constants.CinemaAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            if (!isAuthorized
                && currentUserId != Cinema.CinemaOwnerid
                && Cinema.Status != CinemaStatus.Approved)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, CinemaStatus status)
        {
            var cinema = await Context.Cinema.FirstOrDefaultAsync(
                                                      m => m.CinemaID == id);

            if (cinema == null)
            {
                return NotFound();
            }

            var movieOperation = (status == CinemaStatus.Approved)
                                                       ? Operations.Approve
                                                       : Operations.Reject;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, cinema,
                                        movieOperation);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            cinema.Status = status;
            Context.Cinema.Update(cinema);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
