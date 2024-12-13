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

namespace FilmUpMovie.Pages.ShowRooms
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

        public ShowRoom ShowRoom { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ShowRoom == null)
            {
                return NotFound();
            }

            var showroom = await _context.ShowRoom
                .Include(s => s.Cinema)
                .FirstOrDefaultAsync(s => s.ShowRoomID == id);

            if (showroom == null)
            {
                return NotFound();
            }
            else
            {
                ShowRoom = showroom;
            }

            var isAuthorized = User.IsInRole(Constants.ShowRoomManagersRole) ||
                           User.IsInRole(Constants.ShowRoomAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            if (!isAuthorized
                && currentUserId != ShowRoom.RoomOwnerId
                && ShowRoom.Status != ShowRoomStatus.Approved)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, ShowRoomStatus status)
        {
            var showroom = await Context.ShowRoom.FirstOrDefaultAsync(
                                                      m => m.ShowRoomID == id);

            if (showroom == null)
            {
                return NotFound();
            }

            var showroomOperation = (status == ShowRoomStatus.Approved)
                                                       ? Operations.Approve
                                                       : Operations.Reject;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, showroom,
										showroomOperation);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }
            showroom.Status = status;
            Context.ShowRoom.Update(showroom);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
