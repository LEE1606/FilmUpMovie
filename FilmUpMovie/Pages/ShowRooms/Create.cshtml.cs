using FilmUpMovie.Authorization;
using FilmUpMovie.Data;
using FilmUpMovie.Models;
using FilmUpMovie.Pages.Movies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FilmUpMovie.Pages.ShowRooms
{
    [Authorize(Roles = "ShowRoomAdministrator")]
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
            ViewData["CinemaName"] = new SelectList(_context.Cinema, "CinemaID", "Name");

            return Page();
        }

        [BindProperty]
        public ShowRoom ShowRoom { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ShowRoom.RoomOwnerId = UserManager.GetUserId(User);

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                        User, ShowRoom,
                                                        Operations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            _context.ShowRoom.Add(ShowRoom);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}