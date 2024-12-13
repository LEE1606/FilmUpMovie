using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FilmUpMovie.Data;
using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FilmUpMovie.Authorization;

namespace FilmUpMovie.Pages.ShowTimes
{
    [Authorize(Roles = "ShowTimeAdministrators")]
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
            ViewData["MovieTitle"] = new SelectList(_context.Movie, "ID", "Title");
			ViewData["RoomExperience"] = new SelectList(_context.ShowRoom, "ShowRoomID", "Experience");
			ViewData["MovieReleaseDate"] = new SelectList(_context.Movie, "ID", "ReleaseDate");

			return Page();
        }

        [BindProperty]
        public ShowTime ShowTime { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ShowTime.TimeOwnerId = UserManager.GetUserId(User);

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                        User, ShowTime,
                                                        Operations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            _context.ShowTime.Add(ShowTime);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
