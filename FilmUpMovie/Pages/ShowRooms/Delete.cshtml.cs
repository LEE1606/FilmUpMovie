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

namespace FilmUpMovie.Pages.ShowRooms
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
        public ShowRoom ShowRoom { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ShowRoom == null)
            {
                return NotFound();
            }

            var showroom = await _context.ShowRoom
                .Include(s => s.Cinema)
                .FirstOrDefaultAsync(m => m.ShowRoomID == id);

            if (showroom == null)
            {
                return NotFound();
            }
            else
            {
                ShowRoom = showroom;
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                 User, ShowRoom,
                                                 Operations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.ShowRoom == null)
            {
                return NotFound();
            }
            var showroom = await _context.ShowRoom.FindAsync(id);

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                 User, showroom,
                                                 Operations.Delete);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            if (showroom != null)
            {
                ShowRoom = showroom;
                _context.ShowRoom.Remove(ShowRoom);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
