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

namespace FilmUpMovie.Pages.ShowTimes
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
        public ShowTime ShowTime { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ShowTime == null)
            {
                return NotFound();
            }

            var showtime = await _context.ShowTime.FirstOrDefaultAsync(m => m.ShowTimeID == id);
            if (showtime == null)
            {
                return NotFound();
            }
            ShowTime = showtime;

            ViewData["MovieTitle"] = new SelectList(_context.Movie, "ID", "Title");
            ViewData["RoomExperience"] = new SelectList(_context.ShowRoom, "ShowRoomID", "Experience");
            ViewData["MovieReleaseDate"] = new SelectList(_context.Movie, "ID", "ReleaseDate");

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
            var showtime = await _context
                .ShowTime.AsNoTracking()
                .FirstOrDefaultAsync(m => m.ShowTimeID == id);

            if (showtime == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                     User, showtime,
                                                     Operations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            ShowTime.TimeOwnerId = showtime.TimeOwnerId;

            _context.Attach(ShowTime).State = EntityState.Modified;

            if (ShowTime.Status == ShowTimeStatus.Approved)
            {
                // If the contact is updated after approval, 
                // and the user cannot approve,
                // set the status back to submitted so the update can be
                // checked and approved.
                var canApprove = await AuthorizationService.AuthorizeAsync(User,
                                        ShowTime,
                                        Operations.Approve);

                if (!canApprove.Succeeded)
                {
                    ShowTime.Status = ShowTimeStatus.Submitted;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShowTimeExists(ShowTime.ShowTimeID))
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

        private bool ShowTimeExists(int id)
        {
            return _context.ShowTime.Any(e => e.ShowTimeID == id);
        }
    }
}
