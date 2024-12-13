using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilmUpMovie.Data;
using FilmUpMovie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FilmUpMovie.Authorization;

namespace FilmUpMovie.Pages.ShowRooms
{
    public class EditModel : DI_BasePageModel
    {
        private readonly FilmUpMovie.Data.FilmUpMovieContext _context;

        public EditModel(FilmUpMovieContext context, IConfiguration configuration,
        IAuthorizationService authorizationService,
        UserManager<IdentityUser> userManager) : base(context, authorizationService, userManager)
        {
            _context = context;
        }

        [BindProperty]
        public ShowRoom ShowRoom { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ShowRoom == null)
            {
                return NotFound();
            }

            var showRoom = await _context.ShowRoom.FirstOrDefaultAsync(m => m.ShowRoomID == id);
            if (showRoom == null)
            {
                return NotFound();
            }
            ShowRoom = showRoom;

            ViewData["CinemaName"] = new SelectList(_context.Cinema, "CinemaID", "Name");

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Fetch Contact from DB to get OwnerID.
            var showRoom = await _context
                .ShowRoom.AsNoTracking()
                .FirstOrDefaultAsync(m => m.ShowRoomID == id);

            if (showRoom == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                     User, showRoom,
                                                     Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            ShowRoom.RoomOwnerId = showRoom.RoomOwnerId;

            _context.Attach(ShowRoom).State = EntityState.Modified;

            if (ShowRoom.Status == ShowRoomStatus.Approved)
            {
                // If the contact is updated after approval, 
                // and the user cannot approve,
                // set the status back to submitted so the update can be
                // checked and approved.
                var canApprove = await AuthorizationService.AuthorizeAsync(User,
                                        ShowRoom,
                                        Operations.Approve);

                if (!canApprove.Succeeded)
                {
                    ShowRoom.Status = ShowRoomStatus.Submitted;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShowRoomExists(ShowRoom.ShowRoomID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ShowRoomExists(int id)
        {
            return _context.ShowRoom.Any(e => e.ShowRoomID == id);
        }
    }
}
