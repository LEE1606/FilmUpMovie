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

namespace FilmUpMovie.Pages.ShowTimes
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

			var isAuthorized = await AuthorizationService.AuthorizeAsync(
												 User, ShowTime,
                                                 Operations.Delete);
			if (!isAuthorized.Succeeded)
			{
				return Forbid();
			}

			return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.ShowTime == null)
            {
                return NotFound();
            }
            var showtime = await _context.ShowTime.FindAsync(id);

			var isAuthorized = await AuthorizationService.AuthorizeAsync(
												 User, showtime,
                                                 Operations.Delete);
			if (!isAuthorized.Succeeded)
			{
				return Forbid();
			}

			if (showtime != null)
            {
                ShowTime = showtime;
                _context.ShowTime.Remove(ShowTime);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
