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

namespace FilmUpMovie.Pages.ShowTimes
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

        public ShowTime ShowTime { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ShowTime == null)
            {
                return NotFound();
            }

            var showtime = await _context.ShowTime
                .Include(s => s.Movie)
                .Include(s => s.ShowRoom)
                .FirstOrDefaultAsync(s => s.ShowTimeID == id);

            if (showtime == null)
            {
                return NotFound();
            }
            else
            {
                ShowTime = showtime;
            }

            var isAuthorized = User.IsInRole(Constants.ShowTimeManagersRole) ||
                           User.IsInRole(Constants.ShowTimeAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            if (!isAuthorized
                && currentUserId != ShowTime.TimeOwnerId
                && ShowTime.Status != ShowTimeStatus.Approved)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, ShowTimeStatus status)
        {
            var showtime = await Context.ShowTime.FirstOrDefaultAsync(
                                                      m => m.ShowTimeID == id);

            if (showtime == null)
            {
                return NotFound();
            }

            var showtimeOperation = (status == ShowTimeStatus.Approved)
                                                       ? Operations.Approve
                                                       : Operations.Reject;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, showtime,
                                        showtimeOperation);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            showtime.Status = status;
            Context.ShowTime.Update(showtime);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
